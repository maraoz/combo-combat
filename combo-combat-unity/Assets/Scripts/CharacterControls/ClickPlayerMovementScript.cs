using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickPlayerMovementScript : MonoBehaviour {

    public GameObject clickFeedback;
    public float wallDrawResolution = 1.0f;
    public int wallDrawMaxPoints = 5;
    public float wallMaxLength = 6.0f;

    private float wallLength;

    private Camera referencedCamera;
    private Mage player;

    private ControlState oldState;
    private ControlState state;

    private List<Vector3> points = new List<Vector3>();

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
        wallLength = 0;
    }


    private bool CanIssueCommands() {
        return !GuiUtils.IsGUIFocused() && !player.IsDying();
    }

    void Update() {
        if (!CanIssueCommands()) {
            return;
        }

        // KEYBOARD
        if (Input.GetKeyDown(Hotkeys.FIREBALL_HOTKEY)) {
            state = ControlState.targetingFireball;
        } else if (Input.GetKeyDown(Hotkeys.WALL_HOTKEY)) {
            state = ControlState.drawingWall;
        }


        // MOUSE
        bool rightPressed = Input.GetMouseButton(MouseButton.RIGHT);
        bool rightDown = Input.GetMouseButtonDown(MouseButton.RIGHT);
        bool leftPressed = Input.GetMouseButton(MouseButton.LEFT);
        bool leftDown = Input.GetMouseButtonDown(MouseButton.LEFT);
        bool leftUp = Input.GetMouseButtonUp(MouseButton.LEFT);
        Vector3 mousePos = Input.mousePosition;

        if (rightPressed || leftPressed || leftUp) {
            if (state != ControlState.moving && rightPressed) {
                state = ControlState.moving;
            }
            Vector2 screenPosition = Input.mousePosition;
            Ray ray = referencedCamera.ScreenPointToRay(new Vector3(screenPosition.x, screenPosition.y, 1));


            RaycastHit hitInfo = new RaycastHit();
            int clickableLayer = GameConstants.LAYER_MASK_CLICKABLE;
            Physics.Raycast(ray, out hitInfo, Mathf.Infinity, clickableLayer);


            if (hitInfo.collider == null || !hitInfo.collider.gameObject.CompareTag("Enemy")) {

                float distanceToFloorY = (this.transform.position.y - ray.origin.y) / ray.direction.y;
                Vector3 planePosition = ray.origin + distanceToFloorY * ray.direction;


                bool giveFeedback = false;
                // right click handler
                if (rightPressed) {
                    giveFeedback = true;
                    player.PlanMove(planePosition);
                }

                // left click handler
                if (leftDown) {
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
                if (leftPressed) {
                    switch (state) {
                        case ControlState.drawingWall:
                            int count = points.Count;
                            if (count == 0) {
                                points.Add(planePosition);
                            } else {
                                float distanceToLast = Vector3.Distance(points[count - 1], planePosition);
                                if (distanceToLast > wallDrawResolution) {
                                    if (wallLength + distanceToLast <= wallMaxLength) {
                                        points.Add(planePosition);
                                        wallLength += distanceToLast;
                                        if (wallLength + wallDrawResolution >= wallMaxLength) {
                                            DoCastWall();
                                        }
                                    } else {
                                        // TODO: agregar que se complete lo que falto hasta maxLength
                                        DoCastWall();
                                    }
                                }
                            }
                            break;
                    }
                }
                if (leftUp) {
                    switch (state) {
                        case ControlState.drawingWall:
                            DoCastWall();
                            break;
                    }
                }

                // click feedback
                if (hitInfo.collider != null && (rightDown || leftDown)) {
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

    private void DoCastWall() {
        if (points.Count > 1) {
            player.CastWall(points);
        }
        points.Clear();
        wallLength = 0;
        state = ControlState.moving;
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
            case ControlState.drawingWall:
            case ControlState.targetingFireball:
                MouseCursor.main.SetAttackCursor();
                break;
            default:
                throw new System.NotImplementedException();

        }
    }

}