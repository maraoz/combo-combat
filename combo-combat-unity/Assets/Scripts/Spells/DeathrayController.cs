using UnityEngine;
using System.Collections;

public class DeathrayController : MonoBehaviour {

    public float secondsUntilExhaust = 3.0f;
    public float damage = 50;
    public float rayWidth = 0.2f;
    public float rayLength = 100;
    public Color rayColor = Color.red;

    private float secondsPast;
    private LineRenderer lineRenderer;

    private MageLifeController caster;

    void Awake() {
        secondsPast = 0.0f;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(rayColor, rayColor);
        lineRenderer.SetWidth(rayWidth, rayWidth);
        lineRenderer.SetVertexCount(2);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + forward * rayLength);
    }

    public void SetCaster(MageLifeController mage) {
        caster = mage;
    }

    public MageLifeController GetCaster() {
        return caster;
    }

    void Update() {
        secondsPast += Time.deltaTime;
        if (secondsPast >= secondsUntilExhaust) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (Network.isServer) {
            if (other.tag == GameConstants.MAGE_TAG) {
                MageLifeController mageLife = other.gameObject.GetComponent<MageLifeController>();
                mageLife.DoDamage(damage, caster);
            }
        }
    }


}