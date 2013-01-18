using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour {

    public float duration = 3f;
    private float timePassed;

    void Start() {
        timePassed = 0;
    }

    void Update() {
        timePassed += Time.deltaTime;
        if (timePassed >= duration) {
            Destroy(gameObject);
        }
    }

}
