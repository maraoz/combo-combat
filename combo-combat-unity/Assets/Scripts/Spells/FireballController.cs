using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour {

    public GameObject explosion;
    public float secondsUntilExhaust = 3.0f;
    public float damage = 10;
    public float speed = .2f;
    public float knockbackMagnitude = 20f;

    private float secondsPast;
    private MageLifeController caster;

    // Use this for initialization
    void Start() {
        secondsPast = 0.0f;
    }

    public void SetCaster(MageLifeController mage) {
        caster = mage;
    }

    public MageLifeController GetCaster() {
        return caster;
    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime;
        secondsPast += Time.deltaTime;
        if (secondsPast >= secondsUntilExhaust) {
            DestroySafe();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == GameConstants.MAGE_TAG) {
            MageLifeController mageLife = other.gameObject.GetComponent<MageLifeController>();
            Mage mage = other.gameObject.GetComponent<Mage>();
            mageLife.DoDamage(damage, caster);
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            mage.ApplyKnockback(forward*knockbackMagnitude);
        }
        if (other.tag != GameConstants.HEART_TAG) {
            GameObject.Instantiate(explosion, transform.position, Quaternion.identity);
            DestroySafe();
        }
    }

    void DestroySafe() {
        Destroy(gameObject);
    }

    void OnNetworkInstantiate(NetworkMessageInfo info) {
        Network.RemoveRPCs(info.sender, GameConstants.FIREBALL_GROUP);
    }

}