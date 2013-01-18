using UnityEngine;
using System.Collections;

public class ParticleStarter : MonoBehaviour {

    public float initTime = 1f;
    public ParticleEmitter emitter;

    void Start() {
        for (int i = 0; i < initTime; i++) {
            emitter.Simulate(1.0f);
        }
        emitter.Simulate(initTime - (int) initTime);
    }

}