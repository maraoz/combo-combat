using UnityEngine;
using System.Collections;

public abstract class SpellCaster : MonoBehaviour {

    public int id;
    public float fullCastingTime = 2;
    public float preCastingTime = 1;
    public string animationName = "punch";
    public float cooldown = 5.0f;
    public Texture2D icon;
    public Tooltip tooltip;
    public UserInputController.ControlState inputControlState;
    public AudioClip[] castShouts;
    public float shoutProbability = 1.0f;

    private float lastCastTimestamp;
    private float castingTime = 0f;
    private bool isCasting = false;
    private bool hasCastedSpell = false;
    private Mage mage;
    private UserInputController controls;

    internal virtual void Awake() {
        mage = GetComponent<Mage>();
        controls = GetComponent<UserInputController>();
        lastCastTimestamp = 0f;
    }

    void Update() {
        if (isCasting) {
            castingTime += Time.deltaTime;
            if (castingTime >= preCastingTime && !hasCastedSpell) {
                DoCastSpell();
                hasCastedSpell = true;
            }
            if (castingTime >= fullCastingTime) {
                ResetCaster();
                OnFinishCasting();
            }
        }
    }

    private void ResetCaster() {
        mage.FinishedCasting();
        controls.FinishedCasting();
        castingTime = 0f;
        isCasting = false;
        hasCastedSpell = false;
    }

    protected void PlanCast() {
        float now = Time.time;
        if (IsCooldownActive(now)) {
            return;
        }
        lastCastTimestamp = now;
        if (!isCasting) {
            isCasting = true;
            if (castShouts.Length > 0 && Random.value < shoutProbability) {
                mage.audio.clip = castShouts[(int) (Random.value * castShouts.Length)];
                if (!mage.IsMine()) {
                    mage.audio.volume = 0.3f;
                }
                mage.audio.Play();
            }
            mage.OnSpellStartedCasting(this);
        }
    }

    internal bool IsCooldownActive(float now) {
        return (lastCastTimestamp != 0f && now - lastCastTimestamp < cooldown);
    }

    internal float GetCooldownPercentage() {
        float now = Time.time;
        if (!IsCooldownActive(now)) {
            return 0f;
        }
        return 1 - (now - lastCastTimestamp) / cooldown;
    }

    public void InterruptSpell() {
        ResetCaster();
    }

    public bool IsCasting() {
        return isCasting;
    }

    protected Mage GetMage() {
        return mage;
    }

    internal int GetId() {
        return id;
    }

    public float GetFullCastingTime() {
        return fullCastingTime;
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
