using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickPlayerMovementScript : MonoBehaviour {

    public GameObject clickFeedback;
    public float wallDrawResolution = 1.0f;
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


    public void SimulateSpellHotkey(SpellCaster spell) {
        if (spell is FireballCaster) {
            state = ControlState.targetingFireball;
        } else if (spell is WallCaster) {
            state = ControlState.drawingWall;
        } else {
            Debug.Log("Unknown spell");
        }
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

        if (Input.GetKeyDown(Hotkeys.STOP_HOTKEY)) {
            player.PlanStop();
        }


        // MOUSE
        bool rightPressed = Input.GetMouseButton(MouseButton.RIGHT);
        bool rightDown = Input.GetMouseButtonDown(MouseButton.RIGHT);
        bool leftPressed = Input.GetMouseButton(MouseButton.LEFT);
        bool leftDown = Input.GetMouseButtonDown(MouseButton.LEFT);
        bool leftUp = Input.GetMouseButtonUp(MouseButton.LEFT);

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
                    player.PlanMove(planePosition);
                }
                if (rightDown) {
                    giveFeedback = true;
                }

                // left click handler
                if (leftDown) {
                    switch (state) {
                        case ControlState.targetingFireball:
                            DoCastFireball(planePosition);
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
                                        CompleteWall(planePosition, wallMaxLength - wallLength);
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
                            CompleteWall(planePosition, wallMaxLength - wallLength);
                            DoCastWall();
                            break;
                    }
                }

                // click feedback
                if (hitInfo.collider != null && (rightDown || leftDown)) {
                    Vector3 clickFeedbackPosition = hitInfo.point + Vector3.up * 0.1f;
                    if (giveFeedback) {
                        GameObject feedback = GameObject.Instantiate(clickFeedback, clickFeedbackPosition, Quaternion.identity) as GameObject;
                        if (leftDown) {
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

    private void CompleteWall(Vector3 point, float remainingDistance) {
        int count = points.Count;
        if (count > 1) {
            Vector3 last = points[count - 1];
            float realDistance = Vector3.Distance(last, point);
            Vector3 correction = Vector3.Lerp(last, point, remainingDistance / realDistance);
            points.Add(correction);
        }
    }

    private void DoCastFireball(Vector3 target) {
        player.PlanCastFireball(target);
        state = ControlState.moving;
    }

    private void DoCastWall() {
        if (points.Count > 1) {
            player.PlanCastWall(points);
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
                MouseCursor.SetMoveCursor();
                break;
            case ControlState.drawingWall:
            case ControlState.targetingFireball:
                MouseCursor.SetAttackCursor();
                break;
            default:
                throw new System.NotImplementedException();

        }
    }

}