using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickPlayerMovementScript : MonoBehaviour {

    public GameObject clickFeedback;

    private Mage player;
    private List<SpellCaster> spells;
    private SpellCaster currentSpell;

    private ControlState oldState;
    private ControlState state;

    public enum ControlState {
        moving = 0,
        targetingFireball,
        drawingWall
    }

    void Start() {
        if (!networkView.isMine) {
            return;
        }
        this.player = this.gameObject.GetComponent<Mage>();
        this.spells = player.GetSpellCasters();
        this.currentSpell = null;

        state = ControlState.moving;
        oldState = ControlState.moving;
    }


    private bool CanIssueCommands() {
        return !GuiUtils.IsGUIFocused() && !player.IsDying();
    }


    public bool OnSpellHotkeyPressed(SpellCaster spell) {
        float now = Time.time;
        if (spell.IsCooldownActive(now) || spell.IsCasting()) {
            return false;
        }
        if (currentSpell != null) {
            currentSpell.OnFinishPerforming();
        }
        currentSpell = spell;
        state = spell.GetInputControlState();
        return true;
    }

    void Update() {
        UpdateMouseCursor();

        if (!CanIssueCommands()) {
            if (!player.IsDying() && currentSpell != null) {
                // if focus was lost to GUI/HUD, let spell know
                currentSpell.OnInputFocusLost();
            }
            return;
        }

        // KEYBOARD
        foreach (SpellCaster spell in spells) {
            if (Input.GetKeyDown(spell.GetHotkey())) {
                if (OnSpellHotkeyPressed(spell)) {
                    break;
                }
            }
        }
        if (Input.GetKeyDown(Hotkeys.STOP_HOTKEY)) {
            player.PlanStop(transform.position);
        }


        // MOUSE
        bool rightPressed = Input.GetMouseButton(MouseButton.RIGHT);
        bool rightDown = Input.GetMouseButtonDown(MouseButton.RIGHT);
        bool leftPressed = Input.GetMouseButton(MouseButton.LEFT);
        bool leftDown = Input.GetMouseButtonDown(MouseButton.LEFT);
        bool leftUp = Input.GetMouseButtonUp(MouseButton.LEFT);

        if (rightPressed || leftPressed || leftUp) {
            if (state != ControlState.moving && rightPressed) {
                if (!currentSpell.IsCasting()) {
                    state = ControlState.moving;
                }
            }
            Vector2 screenPosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenPosition.x, screenPosition.y, 1));


            RaycastHit hitInfo = new RaycastHit();
            int clickableLayer = GameConstants.LAYER_MASK_CLICKABLE;
            Physics.Raycast(ray, out hitInfo, Mathf.Infinity, clickableLayer);


            if (hitInfo.collider == null || !hitInfo.collider.gameObject.CompareTag("Enemy")) {

                float distanceToFloorY = (this.transform.position.y - ray.origin.y) / ray.direction.y;
                Vector3 planePosition = ray.origin + distanceToFloorY * ray.direction;


                bool giveFeedback = false;
                // right click handler
                if (rightPressed) {
                    player.PlanMove(transform.position, planePosition);
                }
                if (rightDown) {
                    giveFeedback = true;
                }

                // left click handler for spells
                if (currentSpell != null && !currentSpell.IsCasting()) {
                    if (leftDown && currentSpell != null) {
                        giveFeedback = true;
                        currentSpell.OnClickDown(planePosition);
                    }
                    if (leftPressed && currentSpell != null) {
                        currentSpell.OnClickDragged(planePosition);
                    }
                    if (leftUp && currentSpell != null) {
                        currentSpell.OnClickUp(planePosition);
                    }

                }
                // click feedback
                if (hitInfo.collider != null && (rightDown || leftDown)) {
                    Vector3 clickFeedbackPosition = hitInfo.point + Vector3.up * 0.1f;
                    if (giveFeedback) {
                        GameObject feedback = GameObject.Instantiate(clickFeedback, clickFeedbackPosition, Quaternion.identity) as GameObject;
                        feedback.GetComponent<ClickFeedback>().SetColor(leftDown ? Color.red : Color.green);
                    }
                }
            }
        }
    }

    internal void FinishedCasting() {
        currentSpell = null;
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