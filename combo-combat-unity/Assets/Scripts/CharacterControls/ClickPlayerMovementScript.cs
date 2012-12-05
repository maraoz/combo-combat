using UnityEngine;
using System.Collections;

public class ClickPlayerMovementScript : MonoBehaviour {

    public GameObject clickFeedback;

    private Camera referencedCamera;
    private Mage player;

    private ControlState oldState;
    private ControlState state;

    public enum ControlState {
        moving = 0,
        targetingFireball,
        drawingWall
    }

    void Awake() {
        this.player = this.gameObject.GetComponent<Mage>();
        referencedCamera = Camera.main;
        state = ControlState.moving;
        oldState = ControlState.moving;
    }


    private bool CanIssueCommands() {
        return !GuiUtils.IsGUIFocused() && !player.IsDying();
    }

    void Update() {
        if (!CanIssueCommands()) {
            return;
        }

        if (Input.GetKeyDown(Hotkeys.FIREBALL_HOTKEY)) {
            state = ControlState.targetingFireball;
        }

        bool rightPressed = Input.GetMouseButton(MouseButton.RIGHT);
        bool leftDown = Input.GetMouseButtonDown(MouseButton.LEFT);

        if (rightPressed || leftDown) {
            if (state != ControlState.moving && rightPressed) {
                state = ControlState.moving;
            }
            Vector2 screenPosition = Input.mousePosition;
            Ray ray = referencedCamera.ScreenPointToRay(new Vector3(screenPosition.x, screenPosition.y, 1));


            RaycastHit hitInfo = new RaycastHit();
            int allButUnclickable = GameConstants.Invert(GameConstants.LAYER_MASK_UNCLICKABLE);
            Physics.Raycast(ray, out hitInfo, Mathf.Infinity, allButUnclickable);


            if (hitInfo.collider == null || !hitInfo.collider.gameObject.CompareTag("Enemy")) {

                float distanceToFloorY = (this.transform.position.y - ray.origin.y) / ray.direction.y;
                Vector3 planePosition = ray.origin + distanceToFloorY * ray.direction;


                bool giveFeedback = false;
                // right click handler
                if (Input.GetMouseButton(MouseButton.RIGHT)) {
                    giveFeedback = true;
                    player.PlanMove(planePosition);
                }

                // left click handler
                if (Input.GetMouseButtonDown(MouseButton.LEFT)) {
                    switch (state) {
                        case ControlState.targetingFireball:
                            player.CastFireball(planePosition);
                            state = ControlState.moving;
                            giveFeedback = true;
                            break;
                        default:
                            break;
                    }
                }

                // click feedback
                if (hitInfo.collider != null && (Input.GetMouseButtonDown(MouseButton.RIGHT) || Input.GetMouseButtonDown(MouseButton.LEFT))) {
                    Vector3 clickFeedbackPosition = hitInfo.point + Vector3.up * 0.1f;
                    if (giveFeedback) {
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

        UpdateMouseCursor();

    }

    private void UpdateMouseCursor() {
        if (oldState == state) {
            return;
        }
        oldState = state;
        switch (state) {
            case ControlState.moving:
                MouseCursor.main.SetMoveCursor();
                break;
            case ControlState.targetingFireball:
                MouseCursor.main.SetAttackCursor();
                break;
            default:
                throw new System.NotImplementedException();

        }
    }

}