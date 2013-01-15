using UnityEngine;
using System.Collections;

public class GrenadeController : MonoBehaviour {

    public GameObject explosion;
    public float secondsUntilExhaust = 3.0f;
    public float damage = 10;
    public float knockbackMagnitude = 50f;
    public float explosionRadius = 5f;

    public Color rangeIndicatorColor = Color.white;
    public int rangeIndicatorResolution = 30;

    private float secondsPast;
    private MageLifeController caster;
    private LineRenderer lineRenderer;

    void Awake() {
        secondsPast = 0.0f;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.SetVertexCount(0);
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(rangeIndicatorColor, rangeIndicatorColor);
        lineRenderer.SetWidth(0.1F, 0.1F);
    }

    public void SetCaster(MageLifeController mage) {
        caster = mage;
    }

    public MageLifeController GetCaster() {
        return caster;
    }

    private string GetTimerString() {
        return "" + (int)(secondsUntilExhaust - secondsPast + 1);
    }

    void Update() {
        secondsPast += Time.deltaTime;
        if (secondsPast >= secondsUntilExhaust) {
            if (Network.isServer) {
                GameObject[] mages = GameObject.FindGameObjectsWithTag(GameConstants.TAG_MAGE);
                foreach (GameObject mageObject in mages) {
                    float dist = Vector3.Distance(mageObject.transform.position, transform.position);
                    if (dist < explosionRadius) {
                        Mage mage = mageObject.GetComponent<Mage>();
                        MageLifeController mageLife = mageObject.GetComponent<MageLifeController>();
                        mageLife.DoDamage(damage, caster);
                        Vector3 away = mage.transform.position - transform.position;
                        mage.ApplyKnockback(mage.transform.position, away * (knockbackMagnitude * (1 - dist / explosionRadius)));
                    }
                }
                ExplodeDestroy(transform.position);
            }
        } else {
            lineRenderer.SetVertexCount(rangeIndicatorResolution);
            int i = 0;
            while (i < rangeIndicatorResolution) {
                float angle = 2 * Mathf.PI * i / rangeIndicatorResolution;
                float x = transform.position.x + Mathf.Cos(angle) * explosionRadius;
                float z = transform.position.z + Mathf.Sin(angle) * explosionRadius;
                float y = transform.position.y;

                lineRenderer.SetPosition(i, new Vector3(x, y, z));
                i++;
            }
        }
    }

    void OnGUI() {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Rect timerRect = new Rect(pos.x - 50 / 2, Screen.height - pos.y, 50, 50);
        GUI.Label(timerRect, GetTimerString());
    }


    [RPC]
    void ExplodeDestroy(Vector3 currentPosition) {
        networkView.ClientsUnbuffered("ExplodeDestroy", currentPosition);
        if (explosion != null) {
            GameObject.Instantiate(explosion, currentPosition, Quaternion.identity);
        }
        Destroy(gameObject);
    }


    [RPC]
    internal void AddForce(Vector3 position, Quaternion rotation, Vector3 direction) {
        networkView.ClientsUnbuffered("AddForce", position, rotation, direction);
        transform.position = position;
        transform.rotation = rotation;
        rigidbody.AddForce(direction * 200 + Vector3.up * 50);
        rigidbody.AddTorque(new Vector3(10f, 50f, 0f));
    }
}