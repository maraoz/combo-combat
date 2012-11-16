using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour {

    private float speed = .2f;

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
        transform.position += transform.TransformDirection(Vector3.forward) * speed;
    }

    void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
    }

    public void SetSpeed(float s) {
        speed = s;
    }

}
