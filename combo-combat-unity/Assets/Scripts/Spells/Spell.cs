using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {

    public int id;
    public SpellCast cast;
    public SpellForm form;
    public SpellNature nature;

    public Texture2D icon;
    public Texture2D frame;
    public Tooltip tooltip;
    public KeyCode hotkey;

    private Mage mage;
    private UserInputController controls;

    public KeyCode GetHotkey() {
        return hotkey;
    }

    internal virtual void Awake() {
        mage = GetComponent<Mage>();
        cast.SetSpell(this);
        form.SetSpell(this);
        controls = GetComponent<UserInputController>();
    }

    internal void ResetCaster() {
        mage.FinishedCasting();
        controls.FinishedCasting();
    }

    internal void PlanCast() {
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
            form.StartCasting();
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


    internal void DoCastSpell() {
        form.DoCastForm(nature);
    }
}
