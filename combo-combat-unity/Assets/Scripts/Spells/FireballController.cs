using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour {

    public GameObject explosion;
    public float secondsUntilExhaust = 3.0f;
    
    private float secondsPast;
    private float speed = .2f;

    // Use this for initialization
    void Start () {
        secondsPast = 0.0f;
    }
    
    // Update is called once per frame
    void Update () {
        transform.position += transform.TransformDirection(Vector3.forward) * speed;
        secondsPast += Time.deltaTime;
        if (secondsPast >= secondsUntilExhaust) {
            DestroySafe();
        }
    }

    void OnTriggerEnter(Collider other) {
        DestroySafe();
        GameObject.Instantiate(explosion, transform.position, Quaternion.identity);
    }

    void DestroySafe() {
        Network.Destroy(gameObject);
        /*if (Network.isServer) {
            Network.Destroy(gameObject);
            Network.RemoveRPCs(networkView.viewID);
        }*/
    }

    public void SetSpeed(float s) {
        speed = s;
    }

}
