using UnityEngine;
using System.Collections;

public class CharacterSimpleAnimation : MonoBehaviour {

    public float runSpeedScale = 1.0f;
    public float walkSpeedScale = 1.0f;

    private Mage player;

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
            animation.CrossFade("Sprint");
        } else if (currentSpeed > 0.1) {
            animation.CrossFade("Walk");
        } else {
            animation.CrossFade("Idle");
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
            animation.wrapMode = WrapMode.ClampForever;
            animation.CrossFade("Deathfall");
        }
        if (player.IsStunned()) {
            animation.wrapMode = WrapMode.ClampForever;
            animation.CrossFade("Gothit");
        }

    }
}
