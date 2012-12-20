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
    private float groundSpeed = 0f; // ground x-z axis speed in the direction the mage is looking at
    private float verticalSpeed = 0f; // y axis speed

    // external forces
    private Vector3 externalForce = Vector3.zero; // external forces applied to mage
    public float amortiguationCoefficient = 0.8f; // damping coefficient for external forces
    public float epsilonMagnitude = 0.1f; // min considerable external force magnitude
    private Vector3 externalVelocity = Vector3.zero; // external velocity

    // spells
    private SpellCaster currentSpellBeingCasted;
    private FireballCaster fireballCaster;
    private WallCaster wallCaster;

    void Awake() {
        life = GetComponent<MageLifeController>();
        controller = GetComponent<CharacterController>();
        currentSpellBeingCasted = null;
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
        if (target != Vector3.zero && currentSpellBeingCasted == null) {
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

    void ApplyExternalForces() {
        // external force instant application
        externalVelocity += externalForce;
        // amortiguation
        externalVelocity -= externalVelocity  * amortiguationCoefficient * Time.deltaTime;
        if (externalForce.magnitude > 0) {
            externalForce = Vector3.zero;
        } 

        // update external velocity
        if (externalVelocity.magnitude < epsilonMagnitude) {
            externalVelocity = Vector3.zero;
        }

    }


    void Update() {
        // vertical movement
        ApplyGravity();
        Vector3 verticalVelocity = new Vector3(0, verticalSpeed, 0);

        // ground movement
        ApplyTargetHunt();
        ApplyExternalForces();
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 groundVelocity = groundSpeed * forward;

        // compute total movement
        Vector3 totalVelocity = verticalVelocity + groundVelocity + externalVelocity;

        // apply movement for this period of time
        collisionFlags = controller.Move(totalVelocity * Time.deltaTime);
    }

    public void PlanMove(Vector3 v) {
        target = v;
    }

    public void OnSpellStartedCasting(SpellCaster spell) {
        currentSpellBeingCasted = spell;
    }

    public float GetSpeed() {
        return controller.velocity.magnitude;
    }

    public void OnDied() {
        enabled = false;
        currentSpellBeingCasted = null;
    }

    public void OnRespawn() {
        enabled = true;
        currentSpellBeingCasted = null;
        target = Vector3.zero;
    }

    public bool IsDying() {
        return life.IsDying();
    }

    public bool IsGrounded() {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }


    public bool IsCasting() {
        return currentSpellBeingCasted != null;
    }

    public SpellCaster GetCurrentSpellCaster() {
        return currentSpellBeingCasted;
    }

    internal void FinishedCasting() {
        currentSpellBeingCasted = null;
    }

    internal void PlanStop() {
        target = Vector3.zero;
    }

    internal List<SpellCaster> GetSpellCasters() {
        List<SpellCaster> ret = new List<SpellCaster>();
        ret.Add(fireballCaster);
        ret.Add(wallCaster);
        return ret;
    }

    internal void ApplyKnockback(Vector3 force) {
        externalForce += force;
    }
}