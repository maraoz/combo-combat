using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour {

    public GameObject explosion;
    public float secondsUntilExhaust = 3.0f;
    public float damage = 10;
    public float speed = .2f;

    private float secondsPast;

    // Use this for initialization
    void Start() {
        secondsPast = 0.0f;
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
        DestroySafe();
        GameObject.Instantiate(explosion, transform.position, Quaternion.identity);
        if (other.tag == GameConstants.MAGE_TAG) {
            MageLifeController mage = other.gameObject.GetComponent<MageLifeController>();
            mage.DoDamage(damage);
        }
    }

    void DestroySafe() {
        Destroy(gameObject);
        /*if (Network.isServer) {
            Network.Destroy(gameObject);
        }*/
    }

    void OnNetworkInstantiate(NetworkMessageInfo info) {
        Network.RemoveRPCs(info.sender, GameConstants.FIREBALL_GROUP);
    }

}