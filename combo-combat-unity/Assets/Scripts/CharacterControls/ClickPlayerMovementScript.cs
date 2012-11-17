using UnityEngine;
using System.Collections;

public class ClickPlayerMovementScript : MonoBehaviour {

    private Mage player;
    public float floorYOffset = -1.0f;
    private Camera referencedCamera;

    void Awake() {
        this.player = this.gameObject.GetComponent<Mage>();
        referencedCamera = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        Vector2 screenPosition = Input.mousePosition;
        Ray ray = referencedCamera.ScreenPointToRay(new Vector3(screenPosition.x, screenPosition.y, 1));

        float floorY = this.transform.position.y + floorYOffset;
        float distanceToFloorY = (floorY - ray.origin.y) / ray.direction.y;

        // Check collision
        RaycastHit hitInfo = new RaycastHit();
        if (Input.GetMouseButton(MouseButton.RIGHT) || Input.GetMouseButtonDown(MouseButton.RIGHT) || Input.GetMouseButton(MouseButton.LEFT)) {
            Physics.Raycast(ray, out hitInfo, distanceToFloorY);
        }

        if (Input.GetMouseButton(MouseButton.RIGHT) || Input.GetMouseButtonDown(MouseButton.LEFT)) {
            if (hitInfo.collider == null || !hitInfo.collider.gameObject.CompareTag("Enemy")) {
                Vector3 planePosition = ray.origin + distanceToFloorY * ray.direction;

                if (Input.GetMouseButton(MouseButton.RIGHT)) {
                    player.PlanMove(planePosition);

                }
                if (Input.GetMouseButtonDown(MouseButton.LEFT)) {
                    player.CastFireball(planePosition);
                }
            }
        }

    }

}