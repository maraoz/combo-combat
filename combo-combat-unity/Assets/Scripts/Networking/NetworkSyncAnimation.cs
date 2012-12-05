using UnityEngine;
using System.Collections;
using System;

public class NetworkSyncAnimation : MonoBehaviour {

    public enum AniStates {
        walk = 0,
        run,
        kick,
        punch,
        jump,
        jumpfall,
        idle,
        gotbit,
        gothit,
        walljump,
        deathfall,
        jetpackjump,
        ledgefall,
        buttstomp,
        jumpland
    }

    private AniStates currentAnimation = AniStates.idle;
    private AniStates lastAnimation = AniStates.idle;

    public void SyncAnimation(String animationValue) {
        currentAnimation = (AniStates) Enum.Parse(typeof(AniStates), animationValue);
    }

    void Update() {

        if (lastAnimation != currentAnimation) {
            lastAnimation = currentAnimation;
            animation.CrossFade(Enum.GetName(typeof(AniStates), currentAnimation));
            animation["run"].normalizedSpeed = 1.0F;
            animation["walk"].normalizedSpeed = 1.0F;
        }
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (stream.isWriting) {
            char ani = (char) currentAnimation;
            stream.Serialize(ref ani);
        } else {
            char ani = (char) 0;
            stream.Serialize(ref ani);
            currentAnimation = (AniStates) ani;
        }

    }

}
