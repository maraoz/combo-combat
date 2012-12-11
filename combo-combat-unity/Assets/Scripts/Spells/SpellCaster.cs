using UnityEngine;
using System.Collections;

public abstract class SpellCaster : MonoBehaviour {

    public float fullCastingTime = 2;
    public float preCastingTime = 1;
    public string animationName = "punch";
    public float cooldown = 5.0f;

    private float lastCastTimestamp;
    private float castingTime = 0f;
    private bool isCasting = false;
    private bool hasCastedSpell = false;
    private Mage mage;

    void Awake() {
        mage = GetComponent<Mage>();
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
        castingTime = 0f;
        isCasting = false;
        hasCastedSpell = false;
    }

    protected bool PlanCast() {
        float now = Time.time;
        if (lastCastTimestamp != 0f && now - lastCastTimestamp < cooldown) {
            return false;
        }
        lastCastTimestamp = now;
        if (!isCasting) {
            isCasting = true;
            return true;
        }
        return false;
    }

    public void InterruptSpell() {
        ResetCaster();
    }

    public bool IsCasting() {
        return isCasting;
    }

    public float GetFullCastingTime() {
        return fullCastingTime;
    }

    public string GetAnimationName() {
        return animationName;
    }

    public abstract void DoCastSpell();

    public abstract void OnFinishCasting();


}
