using UnityEngine;
using System.Collections;

public abstract class SpellCaster : MonoBehaviour {

    public int id;
    public Cast cast;
    public string animationName = "punch";
    public Texture2D icon;
    public Texture2D frame;

    public Tooltip tooltip;
    public UserInputController.ControlState inputControlState;
    public AudioClip[] castShouts;
    public float shoutProbability = 1.0f;

    private Mage mage;
    private UserInputController controls;

    internal virtual void Awake() {
        mage = GetComponent<Mage>();
        controls = GetComponent<UserInputController>();
    }

    internal void ResetCaster() {
        mage.FinishedCasting();
        controls.FinishedCasting();
    }

    protected void PlanCast() {
        float now = Time.time;
        if (cast.IsCooldownActive(now)) {
            return;
        }
        if (!mage.IsGrounded()) {
            return;
        }
        cast.SetLastCastTimestamp(now);
        
        if (!cast.IsCasting()) {
            cast.StartCasting();
            if (castShouts.Length > 0 && Random.value < shoutProbability) {
                mage.audio.clip = castShouts[(int) (Random.value * castShouts.Length)];
                mage.audio.Play();
            }
            mage.OnSpellStartedCasting(this);
        }
    }

    public void InterruptSpell() {
        ResetCaster();
    }

    protected Mage GetMage() {
        return mage;
    }

    internal int GetId() {
        return id;
    }

    public string GetAnimationName() {
        return animationName;
    }

    internal Tooltip GetTooltip() {
        return tooltip;
    }

    internal string GetHotkeyString() {
        return ("" + System.Convert.ToChar(GetHotkey())).ToUpper();
    }

    internal Texture GetIcon() {
        return icon;
    }

    internal Texture2D GetFrame() {
        return frame;
    }

    internal UserInputController.ControlState GetInputControlState() {
        return inputControlState;
    }


    // called when pre casting time elapsed and spell effect should be created
    public abstract void DoCastSpell();

    // called when full casting time elapsed
    public abstract void OnFinishCasting();

    // returns the spells hotkey
    public abstract KeyCode GetHotkey();

    // called when spell performance was finished. If performance is correct must call PlanCast (FIX this? maybe returns if performance was right)
    public abstract void OnFinishPerforming();

    // called when user clicks mouse down on the world position
    public abstract void OnClickDown(Vector3 position);

    // called when user drags mouse over world position
    public abstract void OnClickDragged(Vector3 position);

    // called when user releases the mouse on the world position
    public abstract void OnClickUp(Vector3 position);

    // called when input focus is lost to GUI/HUD when casting spell
    public abstract void OnInputFocusLost();

}
