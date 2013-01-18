using UnityEngine;
using System.Collections;

public class WallEffect : MonoBehaviour {

    public float padding = 0f;

    public void SetPadding(float p) {
        padding = p;
    }



    void Update() {

        float sc = transform.localScale.x;
        float realPadding = padding;

        float t = Time.time * 0.1f;
        float v = 0.5f * t;
        float w = 1f * Mathf.Cos(0.2f * t);

        renderer.material.mainTextureOffset =
            new Vector2(-3.37f * t + realPadding, 1);
        renderer.material.SetTextureOffset("_NormalMap",
            new Vector2(v + realPadding/10, w));
        renderer.material.SetTextureScale("_NormalMap",
            new Vector2(sc, 0.5f));

    }
}
