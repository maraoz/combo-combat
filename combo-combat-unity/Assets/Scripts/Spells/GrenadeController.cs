using UnityEngine;
using System.Collections;

public class GrenadeController : MonoBehaviour {

    public GameObject explosion;
    public float secondsUntilExhaust = 3.0f;
    public float damage = 10;
    public float knockbackMagnitude = 50f;
    public float explosionRadius = 5f;

    private float secondsPast;
    private MageLifeController caster;

    void Start() {
        secondsPast = 0.0f;
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
            if (Network.isServer) {
                GameObject[] mages = GameObject.FindGameObjectsWithTag(GameConstants.MAGE_TAG);
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
        }


    }

    [RPC]
    void ExplodeDestroy(Vector3 currentPosition) {
        networkView.ClientsUnbuffered("ExplodeDestroy", currentPosition);
        if (explosion != null) {
            GameObject.Instantiate(explosion, currentPosition, Quaternion.identity);
        }
        Destroy(gameObject);
    }


    internal void AddForce(Vector3 forward) {
        rigidbody.AddForce(forward * 200 + Vector3.up * 250);
        rigidbody.AddTorque(new Vector3(10f, 50f, 0f));
    }
}