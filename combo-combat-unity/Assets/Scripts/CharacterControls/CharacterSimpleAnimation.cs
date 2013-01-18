using UnityEngine;
using System.Collections;

public class CharacterSimpleAnimation : MonoBehaviour {

    public float runSpeedScale = 1.0f;
    public float walkSpeedScale = 1.0f;

    private Mage player;
    private int dieMode;

    void Awake() {
        player = GetComponent<Mage>();

        // By default loop all animations
        animation.wrapMode = WrapMode.Loop;

        // We are in full control here - don't let any other animations play when we start
        animation.Stop();
        animation.Play("Idle");
    }
    void Update() {

        float currentSpeed = player.GetSpeed();
        animation.wrapMode = WrapMode.Loop;

        if (currentSpeed > player.walkingSpeed) {
            animation.CrossFade("Run");
        } else if (currentSpeed > 0.1) {
            animation.CrossFade("Run");
        } else if (player.IsResting()) {
            animation.CrossFade("Idle");
        } else {
            animation.CrossFade("Staff Stance");
        }

        animation["Run"].normalizedSpeed = runSpeedScale;
        animation["Walk"].normalizedSpeed = walkSpeedScale;

        if (player.IsCasting()) {
            SpellCaster spell = player.GetCurrentSpellCaster();
            float fullCastingTime = spell.GetFullCastingTime();
            if (fullCastingTime > 0) {
                string aniName = spell.GetAnimationName();
                AnimationState state = animation[aniName];
                state.speed = state.length / fullCastingTime;
                animation.CrossFade(aniName);
            }
        }
        if (player.IsDying()) {
            if (dieMode == 0) {
                dieMode = (Random.value > 0.5 ? 1 : 2);
            }
            animation.wrapMode = WrapMode.ClampForever;
            animation.CrossFade("Die " + dieMode);
        } else {
            dieMode = 0;
        }
        if (player.IsStunned()) {
            animation.CrossFade("Crouch Idle");
        }
        if (!player.IsGrounded()) {
            animation.CrossFade("Falling");
        }

    }
}
