using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {

    public PlayerState state;
    public float walkingSpeed = 6f;
    public float gravityMagnitude = 20.0f;

    private CharacterController controller;
    private CollisionFlags collisionFlags;

    // Where the player is heading
    private Vector3 target = Vector3.zero;
    // ground x-z axis speed
    private float groundSpeed = 0f;
    // y axis speed
    private float verticalSpeed = 0f;

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
        if (target != Vector3.zero) {
            if (CheckArrivedTarget()) {
                transform.position = target;
                target = Vector3.zero;
                return;
            }
            transform.LookAt(target);
            groundSpeed = walkingSpeed;
        }
    }

    void Update() {

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

    public float GetSpeed() {
        return controller.velocity.magnitude;
    }

    public bool IsJumping() {
        return false;
    }

    public bool IsGrounded() {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

}
