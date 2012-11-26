using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Mage : MonoBehaviour {

    public float walkingSpeed = 6f;
    public float gravityMagnitude = 20.0f;
    public GameObject fireball;

    private CharacterController controller;
    private CollisionFlags collisionFlags;

    private Vector3 target = Vector3.zero; // Where the player is heading
    private float groundSpeed = 0f; // ground x-z axis speed
    private float verticalSpeed = 0f; // y axis speed

    // TODO: sacar esto de aca
    public float castingTimeNeeded = 2;
    private float castingTime = 0f;
    private bool isCasting = false;
    private bool hasCreatedFireball = false;
    private bool isDying = false;

    // death
    private Transform spawnPosition;
    public float deathTime = 15.0f;
    private float deathTimeSpent = 0f;

    void Awake() {
        controller = GetComponent<CharacterController>();
    }

    public void SetSpawnPosition(Transform spawner) {
        spawnPosition = spawner;
    }

    void ApplyGravity() {
        if (IsGrounded())
            verticalSpeed = -gravityMagnitude * 0.2f;
        else
            verticalSpeed -= gravityMagnitude * Time.deltaTime;
    }

    private bool CheckArrivedTarget() {
        return Vector3.Distance(transform.position, target) < 0.05;
    }

    void ApplyTargetHunt() {
        groundSpeed = 0f;
        if (target != Vector3.zero && !isCasting) {
            target.y = transform.position.y;
            if (CheckArrivedTarget()) {
                transform.position = target;
                target = Vector3.zero;
                return;
            }
            transform.LookAt(target);
            groundSpeed = walkingSpeed;
        }
    }

    void UpdateTimers() {
        if (isCasting) {
            castingTime += Time.deltaTime;
            if (castingTime >= (castingTimeNeeded / 2) && !hasCreatedFireball) {
                hasCreatedFireball = true;
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 right = transform.TransformDirection(Vector3.right);
                Vector3 spawnPosition = transform.position + (1.5f * forward) + (1f * Vector3.up) + (0.5f * right);
                Network.Instantiate(fireball, spawnPosition, transform.rotation, GameConstants.FIREBALL_GROUP);
            }
            if (castingTime >= castingTimeNeeded) {
                castingTime = 0f;
                isCasting = false;
                hasCreatedFireball = false;
            }

        }
    }

    void UpdateDeathTimer() {
        deathTimeSpent += Time.deltaTime;
        if (deathTimeSpent >= deathTime) {
            Respawn();
        }
    }

    void Update() {

        if (!isDying) {
            UpdateTimers();

            // vertical movement
            ApplyGravity();
            Vector3 verticalVelocity = new Vector3(0, verticalSpeed, 0);

            // ground movement
            ApplyTargetHunt();
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 groundVelocity = groundSpeed * forward;

            // compute total movement
            Vector3 totalVelocity = verticalVelocity + groundVelocity;

            // apply movement for this period of time
            collisionFlags = controller.Move(totalVelocity * Time.deltaTime);
        } else {
            UpdateDeathTimer();
        }
    }



    public void PlanMove(Vector3 v) {
        target = v;
    }

    public void CastFireball(Vector3 v) {
        if (!isCasting && !isDying) {
            isCasting = true;
            target = Vector3.zero;
            transform.LookAt(v);
        }
    }

    void Respawn() {
        enabled = true;
        isDying = false;
        deathTimeSpent = 0f;
        Camera.main.GetComponent<IsometricCamera>().SetGrayscale(false);
        transform.position = spawnPosition.position;
        transform.rotation = Quaternion.identity;
        target = Vector3.zero;
        SendMessage("RestartLife");
    }

    public float GetSpeed() {
        return controller.velocity.magnitude;
    }

    public bool IsJumping() {
        return false;
    }

    public bool IsCasting() {
        return isCasting;
    }

    public bool IsDying() {
        return isDying;
    }

    public void DoDie() {
        isDying = true;
        Camera.main.GetComponent<IsometricCamera>().SetGrayscale(true);
    }

    public float GetCastingTimeNeeded() {
        return castingTimeNeeded;
    }

    public bool IsGrounded() {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {

        if (networkView.isMine) {
            if (hit.collider.tag == GameConstants.HEART_TAG) {
                HeartController heart = hit.collider.gameObject.GetComponent<HeartController>();
                heart.DestroyedBy(gameObject);
                SendMessage("DoDamage", -heart.healing);
            }
        }
    }

}