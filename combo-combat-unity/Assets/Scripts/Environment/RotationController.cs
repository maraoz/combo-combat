using UnityEngine;
using System.Collections;

public class RotationController : MonoBehaviour {

    public Vector3 rotation;

    private Transform transf;

    void Awake() {
        transf = transform;
    }

    void Update() {
        transf.Rotate(rotation);
    }

}
