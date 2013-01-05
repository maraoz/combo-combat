using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserInputController : MonoBehaviour {

    public GameObject clickFeedback;

    private Mage player;
    private List<SpellCaster> spells;
    private SpellCaster currentSpell;

    private ControlState oldState;
    private ControlState state;

    public enum ControlState {
        moving = 0,
        targetingFireball,
        drawingWall,
        targetingDeathray,
        targetingGrenade
    }

    void Start() {
        this.player = GetComponent<Mage>();
        this.spells = player.GetSpellCasters();
        this.currentSpell = null;

        state = ControlState.moving;
        oldState = ControlState.moving;
    }

    internal void ServerInit() {
        Start();
    }

    private bool CanIssueCommands() {
        return !GuiUtils.IsGUIFocused() && !player.IsDying() && !player.IsStunned();
    }


    void Update() {
        UpdateMouseCursor();

        // KEYBOARD
        if (currentSpell == null) {
            foreach (SpellCaster spell in spells) {
                if (Input.GetKeyDown(spell.GetHotkey())) {
                    if (OnSpellHotkeyPressed(spell)) {
                        break;
                    }
                }
            }
        }
        if (Input.GetKeyDown(Hotkeys.STOP_HOTKEY)) {
            RequestPlanStop();
        }

        if (!CanIssueCommands()) {
            if (!player.IsDying() && !player.IsStunned() && currentSpell != null) {
                // if focus was lost to GUI/HUD, let spell know
                OnInputFocusLostNotify();
            }
            return;
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
                    currentSpell = null;
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
                    RequestPlanMove(planePosition);
                }
                if (rightDown) {
                    giveFeedback = true;
                }

                // left click handler for spells
                if (currentSpell != null && !currentSpell.IsCasting()) {
                    if (leftDown && currentSpell != null) {
                        giveFeedback = true;
                        OnSpellClickDown(planePosition);
                    }
                    if (leftPressed && currentSpell != null) {
                        OnSpellClickDragged(planePosition);
                    }
                    if (leftUp && currentSpell != null) {
                        OnSpellClickUp(planePosition);
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

    public bool OnSpellHotkeyPressed(SpellCaster spell) {
        float now = Time.time;
        if (spell.IsCooldownActive(now)) {
            return false;
        }
        if (currentSpell != null) {
            OnFinishPerformingRequest();
        }
        OnStartSpellPerform(spell.GetId());
        return true;
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
            case ControlState.targetingDeathray:
            case ControlState.targetingFireball:
            case ControlState.targetingGrenade:
                MouseCursor.SetAttackCursor();
                break;
            default:
                throw new System.NotImplementedException();

        }
    }


    // user input controller RPCs
    [RPC]
    void RequestPlanMove(Vector3 target) {
        if (networkView.Server("RequestPlanMove", target)) {
            player.PlanMove(transform.position, target);
        }
    }

    [RPC]
    void RequestPlanStop() {
        if (networkView.Server("RequestPlanStop")) {
            player.PlanStop(transform.position);
        }
    }

    [RPC]
    void OnSpellClickDown(Vector3 planePosition) {
        networkView.Server("OnSpellClickDown", planePosition);
        currentSpell.OnClickDown(planePosition);
    }

    [RPC]
    void OnSpellClickDragged(Vector3 planePosition) {
        networkView.Server("OnSpellClickDragged", planePosition);
        currentSpell.OnClickDragged(planePosition);
    }

    [RPC]
    void OnSpellClickUp(Vector3 planePosition) {
        networkView.Server("OnSpellClickUp", planePosition);
        currentSpell.OnClickUp(planePosition);
    }

    [RPC]
    void OnInputFocusLostNotify() {
        if (networkView.Server("OnInputFocusLostNotify")) {
            currentSpell.OnInputFocusLost();
        }
    }

    [RPC]
    void OnFinishPerformingRequest() {
        networkView.Server("OnFinishPerformingRequest");
        currentSpell.OnFinishPerforming();
    }

    [RPC]
    void OnStartSpellPerform(int spellId) {
        networkView.Server("OnStartSpellPerform", spellId);
        foreach (SpellCaster spell in spells) {
            if (spell.GetId() == spellId) {
                currentSpell = spell;
                state = spell.GetInputControlState();
                break;
            }
        }
    }

}