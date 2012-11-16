using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour {

    public GameObject explosion;
    public float secondsUntilExhaust = 3.0f;
    public float secondsPast;
    
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
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
        GameObject.Instantiate(explosion, transform.position, Quaternion.identity);
    }

    public void SetSpeed(float s) {
        speed = s;
    }

}
