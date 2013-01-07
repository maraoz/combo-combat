using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class Mage : MonoBehaviour {


    // network
    private NetworkPlayer owner; // set only in server
    private bool isMine;

    // messages
    MessageSystem messages;

    // life and death
    private MageLifeController life;

    // movement
    public float walkingSpeed = 6f;
    public float gravityMagnitude = 20.0f;
    private CharacterController controller;
    private Vector3 target = Vector3.zero; // Where the player is heading
    private float groundSpeed = 0f; // ground x-z axis speed in the direction the mage is looking at
    private float verticalSpeed = 0f; // y axis speed

    // external forces
    private Vector3 externalForce = Vector3.zero; // external forces applied to mage
    public float amortiguationCoefficient = 0.8f; // damping coefficient for external forces
    public float epsilonMagnitude = 0.1f; // min considerable external force magnitude
    private Vector3 externalVelocity = Vector3.zero; // external velocity

    // stun effect
    public float stunMinSpeed = 1.5f;
    public float stunDuration = 2f;
    private bool isStunned = false;
    private float stunTimer = 0f;


    // spells
    private SpellCaster currentSpellBeingCasted;
    private FireballCaster fireballCaster;
    private WallCaster wallCaster;
    private DeathrayCaster deathrayCaster;
    private GrenadeCaster grenadeCaster;

    void Awake() {
        messages = GameObject.Find("MessageSystem").GetComponent<MessageSystem>();
        life = GetComponent<MageLifeController>();
        controller = GetComponent<CharacterController>();
        currentSpellBeingCasted = null;
        fireballCaster = GetComponent<FireballCaster>();
        wallCaster = GetComponent<WallCaster>();
        deathrayCaster = GetComponent<DeathrayCaster>();
        grenadeCaster = GetComponent<GrenadeCaster>();
    }

    internal void SetPlayer(NetworkPlayer networkPlayer) {
        owner = networkPlayer;
    }

    internal NetworkPlayer GetPlayer() {
        return owner;
    }

    internal bool IsMine() {
        return isMine;
    }

    internal void TakeOwnership() {
        isMine = true;
    }

    internal bool IsStunned() {
        return isStunned;
    }




    void UpdateEffects() {
        if (isStunned) {
            stunTimer += Time.deltaTime;
            if (stunTimer >= stunDuration) {
                isStunned = false;
            }
        }
    }

    void UpdateGravity() {
        if (IsGrounded())
            verticalSpeed = -gravityMagnitude * 0.2f;
        else
            verticalSpeed -= gravityMagnitude * Time.deltaTime;
    }

    void UpdateTargetHunt() {
        groundSpeed = 0f;
        if (!isStunned && target != Vector3.zero && currentSpellBeingCasted == null) {
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

    private bool CheckArrivedTarget() {
        return Vector3.Distance(transform.position, target) < 0.06;
    }

    void UpdateExternalForces() {
        // external force instant application
        externalVelocity += externalForce;
        // amortiguation
        externalVelocity -= externalVelocity * amortiguationCoefficient * Time.deltaTime;
        if (externalForce.magnitude > 0) {
            externalForce = Vector3.zero;
        }
        // update external velocity
        if (externalVelocity.magnitude < epsilonMagnitude) {
            externalVelocity = Vector3.zero;
        }

    }


    void Update() {
        // calculate effects
        UpdateEffects();

        // vertical movement
        UpdateGravity();
        Vector3 verticalVelocity = new Vector3(0, verticalSpeed, 0);

        // ground movement
        UpdateTargetHunt();
        UpdateExternalForces();
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 groundVelocity = groundSpeed * forward;

        // compute total movement
        Vector3 totalVelocity = verticalVelocity + groundVelocity + externalVelocity;

        // apply movement for this period of time
        controller.Move(totalVelocity * Time.deltaTime);
    }

    public void OnSpellStartedCasting(SpellCaster spell) {
        currentSpellBeingCasted = spell;
    }

    public float GetSpeed() {
        return controller.velocity.magnitude;
    }

    public void OnDied() {
        enabled = false;
    }

    public void OnRespawn() {
        enabled = true;
        externalForce = Vector3.zero;
        externalVelocity = Vector3.zero;
        currentSpellBeingCasted = null;
        target = Vector3.zero;
    }

    public bool IsDying() {
        return life.IsDying();
    }

    public bool IsGrounded() {
        return controller.isGrounded;
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

    internal List<SpellCaster> GetSpellCasters() {
        // TODO: take this to static editor list
        List<SpellCaster> ret = new List<SpellCaster>();
        ret.Add(fireballCaster);
        ret.Add(wallCaster);
        ret.Add(grenadeCaster);
        ret.Add(deathrayCaster);
        return ret;
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        Collider other = hit.collider;
        if (Network.isServer) {
            if (other.tag == GameConstants.WALL_TAG) {
                if (!IsStunned() && GetSpeed() >= stunMinSpeed) {
                    ApplyStun(transform.position);
                }
            } else if (other.tag == GameConstants.GRENADE_TAG) {
                other.gameObject.GetComponent<GrenadeController>()
                    .AddForce(other.gameObject.transform.position,
                    other.gameObject.transform.rotation,
                    transform.forward);
            }
        }
    }


    // Network

    // called on server
    void OnPlayerDisconnected(NetworkPlayer player) {
        if (player == owner) {
            messages.AddSystemMessageBroadcast(life.GetUsername() + " disconnected.");
            Network.Destroy(gameObject);
            Network.RemoveRPCs(networkView.viewID);
        }
    }

    // RPCS
    [RPC]
    internal void PlanMove(Vector3 currentPosition, Vector3 targetPosition) {
        networkView.Clients("PlanMove", currentPosition, targetPosition);
        transform.position = currentPosition; // TODO: A interpolate between current and updated?
        // TODO: B interpolate towards target with time elapsed??
        target = targetPosition;
    }

    [RPC]
    internal void PlanStop(Vector3 currentPosition) {
        networkView.Clients("PlanStop", currentPosition);
        transform.position = currentPosition; // TODO: A interpolate between current and updated?
        target = Vector3.zero;
    }

    [RPC]
    internal void ApplyKnockback(Vector3 currentPosition, Vector3 force) {
        networkView.Clients("ApplyKnockback", currentPosition, force);
        transform.position = currentPosition; // TODO: A interpolate between current and updated?
        // TODO: C interpolate applying force with time elapsed??
        externalForce += force;
    }

    [RPC]
    internal void LookAt(Vector3 currentPosition, Vector3 target) {
        networkView.Clients("LookAt", currentPosition, target);
        transform.position = currentPosition;
        transform.LookAt(target);
    }

    [RPC]
    internal void ApplyStun(Vector3 currentPosition) {
        networkView.ClientsUnbuffered("ApplyStun", currentPosition);
        transform.position = currentPosition;
        target = Vector3.zero;
        isStunned = true;
        stunTimer = 0;
    }

}