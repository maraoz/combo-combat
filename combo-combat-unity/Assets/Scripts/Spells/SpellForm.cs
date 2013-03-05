using UnityEngine;
using System.Collections;

public abstract class SpellForm : MonoBehaviour {

    public string animationName = "punch";
    public UserInputController.ControlState inputControlState;
    public AudioClip[] castShouts;
    public float shoutProbability = 1.0f;

    private Mage mage;
    private Spell spell;

    internal void SetMage(Mage mage) {
        this.mage = mage;
    }

    internal void SetSpell(Spell spell) {
        this.spell = spell;
    }

    internal Spell GetSpell() {
        return spell;
    }

    internal Mage GetMage() {
        return mage;
    }

    public string GetAnimationName() {
        return animationName;
    }

    internal UserInputController.ControlState GetInputControlState() {
        return inputControlState;
    }

    void Update() {

    }

    internal void StartCasting() {
        if (castShouts.Length > 0 && Random.value < shoutProbability) {
            mage.audio.clip = castShouts[(int) (Random.value * castShouts.Length)];
            mage.audio.Play();
        }
    }


    // input managing methods. override to create custom behaviour

    // called when spell performance was finished. If performance is correct must call PlanCast (FIX this? maybe returns if performance was right)
    public virtual void OnFinishPerforming() { }

    // called when user clicks mouse down on the world position
    public virtual void OnClickDown(Vector3 position) { }

    // called when user drags mouse over world position
    public virtual void OnClickDragged(Vector3 position) { }

    // called when user releases the mouse on the world position
    public virtual void OnClickUp(Vector3 position) { }

    // called when input focus is lost to GUI/HUD when casting spell
    public virtual void OnInputFocusLost() { }


    // must be implemented, spell effect logic related
    // called when pre casting time elapsed and spell effect should be created
    public abstract void DoCastForm(SpellNature nature);

    // called when full casting time elapsed
    public abstract void OnFinishCasting();


}
