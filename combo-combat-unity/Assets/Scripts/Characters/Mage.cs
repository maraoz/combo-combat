using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class Mage : MonoBehaviour {

    public float walkingSpeed = 6f;
    public float gravityMagnitude = 20.0f;
    public GameObject fireball;
    public GameObject wall;
    public float wallBrickLength = 1.0f;

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
    private MessageSystem messages;
    private Transform spawnPosition;
    public float deathTime = 15.0f;
    private float deathTimeSpent = 0f;

    void Awake() {
        controller = GetComponent<CharacterController>();
        messages = GameObject.Find("MessageSystem").GetComponent<MessageSystem>();
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
                DoCastFireball();
            }
            if (castingTime >= castingTimeNeeded) {
                FinishCastingFireball();
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

    void DoCastFireball() {
        hasCreatedFireball = true;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 spawnPosition = transform.position + (1.5f * forward) + (1f * Vector3.up) + (0.5f * right);
        GameObject casted = Network.Instantiate(fireball, spawnPosition, transform.rotation, GameConstants.FIREBALL_GROUP) as GameObject;
        casted.GetComponent<FireballController>().SetCaster(GetComponent<MageLifeController>());
    }

    void FinishCastingFireball() {
        castingTime = 0f;
        isCasting = false;
        hasCreatedFireball = false;
    }

    public void PlanMove(Vector3 v) {
        target = v;
    }

    public void CastWall(List<Vector3> points) {
        int count = points.Count;
        for (int i = 0; i < count - 1; i++) {
            Vector3 current = points[i];
            Vector3 next = points[i + 1];

            float dist = Vector3.Distance(current, next);

            int bricksNeeded = (int) (dist / wallBrickLength);
            float adaptedBrickLenght = dist / bricksNeeded;
            for (int j = 0; j < bricksNeeded; j++) {
                Vector3 currentBrick = Vector3.Lerp(current, next, adaptedBrickLenght * j / dist);
                Vector3 nextBrick = Vector3.Lerp(current, next, adaptedBrickLenght * (j + 1) / dist);
                Vector3 middleBrick = (currentBrick + nextBrick) / 2;

                networkView.RPC("SpawnBrick", RPCMode.All, middleBrick, next, adaptedBrickLenght);

            }

        }
    }

    [RPC]
    public void SpawnBrick(Vector3 middleBrick, Vector3 next, float adaptedBrickLenght) {
        GameObject piece = GameObject.Instantiate(wall, middleBrick, Quaternion.identity) as GameObject;
        piece.transform.LookAt(next);
        Vector3 euler = piece.transform.eulerAngles;
        euler.y += 90;
        piece.transform.eulerAngles = euler;
        Vector3 scale = piece.transform.localScale;
        scale.x *= adaptedBrickLenght;
        piece.transform.localScale = scale;
        Vector3 position = piece.transform.position;
        position.y += 1.5f;
        piece.transform.position = position;
    }

    public void PlanCastFireball(Vector3 v) {
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
        GetComponent<MageLifeController>().RestartLife();
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
        if (!isDying) {
            isDying = true;
            isCasting = false;
            messages.AddSystemMessage("You died. Please wait " + deathTime + " seconds to respawn.", false);
            Camera.main.GetComponent<IsometricCamera>().SetGrayscale(true);
        }
    }

    public float GetCastingTimeNeeded() {
        return castingTimeNeeded;
    }

    public bool IsGrounded() {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

}