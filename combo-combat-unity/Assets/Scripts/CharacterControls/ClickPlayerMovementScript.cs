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

    void Update() {

        if (!GuiUtils.IsGUIFocused() && (Input.GetMouseButton(MouseButton.RIGHT) || Input.GetMouseButtonDown(MouseButton.LEFT))) {
            Vector2 screenPosition = Input.mousePosition;
            Ray ray = referencedCamera.ScreenPointToRay(new Vector3(screenPosition.x, screenPosition.y, 1));


            RaycastHit hitInfo = new RaycastHit();
            int allButUnclickable = GameConstants.Invert(GameConstants.LAYER_MASK_UNCLICKABLE);
            Physics.Raycast(ray, out hitInfo, Mathf.Infinity, allButUnclickable);


            if (hitInfo.collider == null || !hitInfo.collider.gameObject.CompareTag("Enemy")) {

                float distanceToFloorY = (this.transform.position.y - ray.origin.y) / ray.direction.y;
                Vector3 planePosition = ray.origin + distanceToFloorY * ray.direction;

                if (Input.GetMouseButton(MouseButton.RIGHT)) {
                    player.PlanMove(planePosition);
                }
                if (Input.GetMouseButtonDown(MouseButton.LEFT)) {
                    player.CastFireball(planePosition);
                }
                if (hitInfo.collider != null && (Input.GetMouseButtonDown(MouseButton.RIGHT) || Input.GetMouseButtonDown(MouseButton.LEFT))) {
                    Vector3 clickFeedbackPosition = hitInfo.point + Vector3.up * 0.1f;
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