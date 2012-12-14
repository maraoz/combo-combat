using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class Mage : MonoBehaviour {


    public float walkingSpeed = 6f;
    public float gravityMagnitude = 20.0f;

    // life
    private MageLifeController life;

    // movement
    private CharacterController controller;
    private CollisionFlags collisionFlags;
    private Vector3 target = Vector3.zero; // Where the player is heading
    private float groundSpeed = 0f; // ground x-z axis speed
    private float verticalSpeed = 0f; // y axis speed

    // spells
    private SpellCaster currentSpellCaster;
    private FireballCaster fireballCaster;
    private WallCaster wallCaster;

    void Awake() {
        life = GetComponent<MageLifeController>();
        controller = GetComponent<CharacterController>();
        currentSpellCaster = null;
        fireballCaster = GetComponent<FireballCaster>();
        wallCaster = GetComponent<WallCaster>();

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
        if (target != Vector3.zero && currentSpellCaster == null) {
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

    public void PlanCastFireball(Vector3 v) {
        if (fireballCaster.PlanCast(v)) {
            currentSpellCaster = fireballCaster;
            target = Vector3.zero;
        }
    }

    public void PlanCastWall(List<Vector3> points) {
        if (wallCaster.PlanCast(points)) {
            currentSpellCaster = wallCaster;
        }
    }


    public float GetSpeed() {
        return controller.velocity.magnitude;
    }

    public void OnDied() {
        enabled = false;
        currentSpellCaster = null;
    }

    public void OnRespawn() {
        enabled = true;
        currentSpellCaster = null;
        target = Vector3.zero;
    }

    public bool IsDying() {
        return life.IsDying();
    }

    public bool IsGrounded() {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }


    public bool IsCasting() {
        return currentSpellCaster != null;
    }

    public SpellCaster GetCurrentSpellCaster() {
        return currentSpellCaster;
    }

    internal void FinishedCasting() {
        currentSpellCaster = null;
    }

    internal void PlanStop() {
        if (!IsCasting()) {
            target = Vector3.zero;
        }
    }
}