using UnityEngine;
using System.Collections;

public class SpellForm : MonoBehaviour {

    public string animationName = "punch";
    public UserInputController.ControlState inputControlState;
    public AudioClip[] castShouts;
    public float shoutProbability = 1.0f;

    private Mage mage;

    public void SetMage(Mage mage) {
        this.mage = mage;
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
