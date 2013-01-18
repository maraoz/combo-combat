using UnityEngine;
using System.Collections;

public class ClickFeedback : MonoBehaviour {

    public int resolution = 10;
    public float initialRadius = 1;
    public float speed = 0.02f;
    private LineRenderer lineRenderer;

    private float radius;

    void Awake() {

        radius = initialRadius;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.SetVertexCount(0);
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(Color.white, Color.white);
        lineRenderer.SetWidth(0.05F, 0.05F);
    }

    void Update() {

        radius -= speed * Time.deltaTime;

        lineRenderer.SetVertexCount(resolution);
        int i = 0;
        while (i < resolution) {
            float angle = 2 * Mathf.PI * i / (resolution - 1);
            float x = transform.position.x + Mathf.Cos(angle) * radius;
            float z = transform.position.z + Mathf.Sin(angle) * radius;
            float y = transform.position.y;

            lineRenderer.SetPosition(i, new Vector3(x, y, z));
            i++;
        }
        if (radius < 0) {
            Destroy(gameObject);
        }
    }

    public void SetColor(Color color) {
        lineRenderer.SetColors(color, color);
    }
}
