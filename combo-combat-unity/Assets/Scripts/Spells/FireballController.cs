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

    void Start() {
        secondsPast = 0.0f;
        Debug.Log(networkView.viewID);
    }

    public void SetCaster(MageLifeController mage) {
        caster = mage;
    }

    public MageLifeController GetCaster() {
        return caster;
    }

    void Update() {
        transform.position += transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime;
        secondsPast += Time.deltaTime;
        if (secondsPast >= secondsUntilExhaust) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) {
        Collider other = collision.collider;
        if (other.tag == GameConstants.HEART_TAG) {
            return;
        }
        if (Network.isServer) {
            if (other.tag == GameConstants.MAGE_TAG) {
                Mage mage = other.gameObject.GetComponent<Mage>();
                MageLifeController mageLife = other.gameObject.GetComponent<MageLifeController>();
                mageLife.DoDamage(damage, caster);
                Vector3 normal = collision.contacts[0].normal;
                mage.ApplyKnockback(mage.transform.position, normal * -knockbackMagnitude);
            }
            CollideDestroy(transform.position);
        }
    }

    [RPC]
    void CollideDestroy(Vector3 currentPosition) {
        networkView.ClientsUnbuffered("CollideDestroy", currentPosition);
        GameObject.Instantiate(explosion, currentPosition, Quaternion.identity);
        Destroy(gameObject);
    }

}