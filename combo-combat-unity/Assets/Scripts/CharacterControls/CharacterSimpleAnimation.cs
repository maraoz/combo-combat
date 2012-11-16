using UnityEngine;
using System.Collections;

public class CharacterSimpleAnimation : MonoBehaviour {

    public float runSpeedScale = 1.0f;
    public float walkSpeedScale = 1.0f;

    void Awake() {
        // By default loop all animations
        animation.wrapMode = WrapMode.Loop;

        // We are in full control here - don't let any other animations play when we start
        animation.Stop();
        animation.Play("idle");
    }
    void Update() {
        Player player = GetComponent<Player>();
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

        if (player.IsJumping()) {
            animation.wrapMode = WrapMode.ClampForever;
            animation.CrossFade("jump", 0.2f);
            SendMessage("SyncAnimation", "jump");
        }
        if (player.IsCasting()) {
            AnimationState state = animation["punch"];
            state.speed = state.length / player.GetCastingTimeNeeded();
            animation.CrossFade("punch");
        }
    }
}
