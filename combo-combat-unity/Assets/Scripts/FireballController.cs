using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour {

    private float speed = .1f;

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
        transform.position += transform.TransformDirection(Vector3.forward) * speed;
    }
}
