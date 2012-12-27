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
        animation.Play("idle");
    }
    void Update() {
        
        float currentSpeed = player.GetSpeed();
        animation.wrapMode = WrapMode.Loop;

        // Fade in run
        if (currentSpeed > player.walkingSpeed) {
            animation.CrossFade("run");
            // We fade out jumpland quick otherwise we get sliding feet
            animation.Blend("jumpland", 0);
            SendMessage("SyncAnimation", "run");
        }
            // Fade in walk
        else if (currentSpeed > 0.1) {
            // changed from walk to run to try
            animation.CrossFade("run");
            // We fade out jumpland realy quick otherwise we get sliding feet
            animation.Blend("jumpland", 0);
            SendMessage("SyncAnimation", "run");
        }
            // Fade out walk and run
        else {
            animation.CrossFade("idle");
            SendMessage("SyncAnimation", "idle");
        }

        animation["run"].normalizedSpeed = runSpeedScale;
        animation["walk"].normalizedSpeed = walkSpeedScale;

        if (player.IsCasting()) {
            SpellCaster spell = player.GetCurrentSpellCaster();
            float fullCastingTime = spell.GetFullCastingTime();
            if (fullCastingTime > 0) {
                string aniName = spell.GetAnimationName();
                AnimationState state = animation[aniName];
                state.speed = state.length / fullCastingTime;
                animation.CrossFade(aniName);
                SendMessage("SyncAnimation", aniName);
            }
        }
        if (player.IsDying()) {
            animation.wrapMode = WrapMode.ClampForever;
            animation.CrossFade("deathfall");
            SendMessage("SyncAnimation", "deathfall");
        }

    }
}
