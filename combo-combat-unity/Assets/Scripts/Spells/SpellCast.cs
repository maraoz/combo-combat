using UnityEngine;
using System.Collections;

public class SpellCast : MonoBehaviour {

    public float fullCastingTime = 2;
    public float preCastingTime = 1;
    public float cooldown = 5.0f;

    private Spell spell;
    private float lastCastTimestamp;
    private float castingTime = 0f;
    private bool isCasting = false;
    private bool hasCastedSpell = false;

    public void SetSpell(Spell spell) {
        this.spell = spell;
    }

    public bool IsCasting() {
        return isCasting;
    }

    internal float GetFullCastingTime() {
        return fullCastingTime;
    }

    void Awake() {
        lastCastTimestamp = 0f;
    }

    void Update() {
        if (isCasting) {
            castingTime += Time.deltaTime;
            if (castingTime >= preCastingTime && !hasCastedSpell) {
                spell.DoCastSpell();
                hasCastedSpell = true;
            }
            if (castingTime >= fullCastingTime) {
                ResetCast();
                spell.form.OnFinishCasting();
            }
        }
    }

    private void ResetCast() {
        castingTime = 0f;
        isCasting = false;
        hasCastedSpell = false;
        spell.ResetCaster();
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

    internal void SetLastCastTimestamp(float now) {
        lastCastTimestamp = now;
    }

    internal void StartCasting() {
        isCasting = true;
    }

}
