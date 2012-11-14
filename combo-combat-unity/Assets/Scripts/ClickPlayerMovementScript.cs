using UnityEngine;
using System.Collections;
/*
public class ClickPlayerMovementScript : MonoBehaviour {

    public Camera referencedCamera;
    private Player player;
    public float floorYOffset = -1.0f;

    public Skill itemPickupSkill;

    private bool startedTargeting;

    void Awake() {
        this.player = this.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update() {
        Vector2 screenPosition = Input.mousePosition;
        Ray ray = referencedCamera.ScreenPointToRay(new Vector3(screenPosition.x, screenPosition.y, 1));

        float floorY = this.transform.position.y + floorYOffset;
        float distanceToFloorY = (floorY - ray.origin.y) / ray.direction.y;

        // Check collision
        RaycastHit hitInfo = new RaycastHit();
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButton(1)) {
            Physics.Raycast(ray, out hitInfo, distanceToFloorY);
        }

        if (Input.GetMouseButton(0) && !startedTargeting) {
            if (hitInfo.collider == null) {
                if (player.state.CanCancel(player)) {
                    if (!startedTargeting) {
                        Vector3 planePosition = ray.origin + distanceToFloorY * ray.direction;
                        player.Move(planePosition);
                    }
                }
            } else if (hitInfo.collider.gameObject.CompareTag("Enemy")) {
                if (player.state.CanCancel(player) && !hitInfo.collider.gameObject.GetComponent<Battler>().isDead()) {
                    // Set current target to this enemy
                    player.Target(hitInfo.collider.gameObject);

                    // Begin regular melee attack script
                    Skill skill = player.getLeftClickSkill();

                    if (player.mp > skill.mpConsumption) {
                        player.ChangeState(skill.beginState(player));
                    }
                }
            } else {
                if (player.state.CanCancel(player)) {
                    if (!startedTargeting) {
                        Vector3 planePosition = ray.origin + distanceToFloorY * ray.direction;
                        player.Move(planePosition);
                    }
                }
            }
        } else if (Input.GetMouseButtonUp(0)) {
            startedTargeting = false;
        }

        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null && hitInfo.collider.gameObject.CompareTag("Item")) {
            if (player.state.CanCancel(player)) {
                startedTargeting = true;
                player.Target(hitInfo.collider.gameObject);

                Skill skill = itemPickupSkill;
                player.ChangeState(skill.beginState(player));
            }
        }

        if (Input.GetMouseButton(1)) {

            Skill skill = player.getRightClickSkill();

            if (skill.isTargetable() && player.mp > skill.mpConsumption) {
                if (hitInfo.collider != null && hitInfo.collider.gameObject.CompareTag("Enemy")) {
                    if (player.state.CanCancel(player) && !hitInfo.collider.gameObject.GetComponent<Battler>().isDead()) {
                        // Set current target to this enemy
                        player.Target(hitInfo.collider.gameObject);

                        player.ChangeState(skill.beginState(player));
                    }
                }
            } else {
                if (player.state.CanCancel(player)) {
                    player.Target(null);
                    player.ChangeState(skill.beginState(player));
                }
            }
        }

    }

}*/