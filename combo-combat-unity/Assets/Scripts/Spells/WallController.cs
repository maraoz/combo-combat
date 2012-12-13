using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour {


    public float duration = 3;
    private float timePassed;

    void Start() {
        timePassed = 0;
    }

    void Update() {
        // Animates main texture scale in a funky way!
        float scaleX = Mathf.Cos(Time.time) * 0.5f + 1;
        float scaleY = Mathf.Sin(Time.time) * 0.5f + 1;
        renderer.material.mainTextureScale = new Vector2(scaleX, scaleY);
        timePassed += Time.deltaTime;
        if (timePassed >= duration) {
            Destroy(gameObject);
        }
    }
}
