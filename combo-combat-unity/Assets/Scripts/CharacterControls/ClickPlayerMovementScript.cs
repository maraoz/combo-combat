using UnityEngine;
using System.Collections;

public class ClickPlayerMovementScript : MonoBehaviour {

    public GameObject clickFeedback;

    private Camera referencedCamera;
    private Mage player;

    void Awake() {
        this.player = this.gameObject.GetComponent<Mage>();
        referencedCamera = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        Vector2 screenPosition = Input.mousePosition;
        Ray ray = referencedCamera.ScreenPointToRay(new Vector3(screenPosition.x, screenPosition.y, 1));

        float distanceToFloorY = (this.transform.position.y - ray.origin.y) / ray.direction.y;

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
                if (Input.GetMouseButtonDown(MouseButton.RIGHT) || Input.GetMouseButtonDown(MouseButton.LEFT)) {
                    Vector3 clickFeedbackPosition = planePosition + Vector3.up * 0.1f;
                    GameObject feedback = GameObject.Instantiate(clickFeedback, clickFeedbackPosition, Quaternion.identity) as GameObject;
                    if (Input.GetMouseButtonDown(MouseButton.LEFT)) {
                        feedback.GetComponent<ClickFeedback>().SetColor(Color.red);
                    } else {
                        feedback.GetComponent<ClickFeedback>().SetColor(Color.green);
                    }
                }
            }
        }

    }

}