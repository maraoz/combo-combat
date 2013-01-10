using UnityEngine;
using System.Collections;

public class HeartEffect : MonoBehaviour {

    public float duration = 1f;
    public float duration2 = 2f;

    private float currentDuration;
    private Quaternion initialOrientation;

    // Use this for initialization
    void Start() {
        currentDuration = 0f;
        initialOrientation = transform.rotation;
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = initialOrientation;
        currentDuration += Time.deltaTime;
        if (currentDuration >= duration && currentDuration < duration2) {
            foreach (Transform t in transform) {
                ParticleEmitter emitter = t.particleEmitter;
                if (emitter != null) {
                    emitter.emit = false;
                }
            }
        } else if (currentDuration >= duration2) {
            Destroy(gameObject);
        }
    }
}
