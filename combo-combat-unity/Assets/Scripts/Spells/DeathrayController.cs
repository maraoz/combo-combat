using UnityEngine;
using System.Collections;

public class DeathrayController : MonoBehaviour {

    public float secondsUntilExhaust = 3.0f;
    public float damage = 50;
    public float rayLength = 100;
    public float rayDamageWidth = 0.5f;
    public Color rayDamageColor = Color.white;
    public float rayWarningWidth = 0.2f;
    public Color rayWarningColor = Color.red;

    private float secondsPast;
    private LineRenderer lineRenderer;
    private Vector3 origin;
    private Vector3 end;

    private MageLifeController caster;

    void Awake() {
        this.enabled = false;
        secondsPast = 0.0f;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(rayWarningColor, rayWarningColor);
        lineRenderer.SetWidth(rayWarningWidth, rayDamageWidth);
        lineRenderer.SetVertexCount(2);
        origin = transform.position;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        end = transform.position + forward * rayLength;
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, end);
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

    internal void ActivateDamage() {
        this.enabled = true;
        lineRenderer.SetColors(rayDamageColor, rayDamageColor);
        lineRenderer.SetWidth(rayDamageWidth, rayDamageWidth);
        if (Network.isServer) {
            RaycastHit[] hits = Physics.SphereCastAll(origin, rayDamageWidth, transform.forward, rayLength);
            foreach (RaycastHit hit in hits) {
                Debug.Log(hit.collider.gameObject.name);
                Collider other = hit.collider;
                if (other.tag == GameConstants.MAGE_TAG) {
                    MageLifeController mageLife = other.gameObject.GetComponent<MageLifeController>();
                    mageLife.DoDamage(damage, caster);
                }
            }
        }
    }

}