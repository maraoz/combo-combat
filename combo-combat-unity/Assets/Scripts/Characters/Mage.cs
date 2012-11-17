using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Mage : MonoBehaviour {

    public float walkingSpeed = 6f;
    public float gravityMagnitude = 20.0f;
    public GameObject fireball; 

    private CharacterController controller;
    private CollisionFlags collisionFlags;

    // Where the player is heading
    private Vector3 target = Vector3.zero;
    // ground x-z axis speed
    private float groundSpeed = 0f;
    // y axis speed
    private float verticalSpeed = 0f;

    // TODO: sacar esto de aca
    public float castingTimeNeeded = 2;
    private float castingTime = 0f;
    private bool isCasting = false;
    private bool hasCreatedFireball = false;

    void Awake() {
        controller = GetComponent<CharacterController>();
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
                Network.Instantiate(fireball, transform.position + (1.5f * forward) + (1f * Vector3.up) + (0.5f*right), transform.rotation, GameConstants.FIREBALL_GROUP);
            }
            if (castingTime >= castingTimeNeeded) {
                castingTime = 0f;
                isCasting = false;
                hasCreatedFireball = false;
            }

        }
    }

    void Update() {

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
    }



    public void PlanMove(Vector3 v) {
        target = v;
    }

    public void CastFireball(Vector3 v) {
        isCasting = true;
        target = Vector3.zero;
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

    public float GetCastingTimeNeeded() {
        return castingTimeNeeded;
    }

    public bool IsGrounded() {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

}
