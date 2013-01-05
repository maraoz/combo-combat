using UnityEngine;
using System.Collections;

public class IsometricCamera : MonoBehaviour {

    Transform target;

    public float maxDistance = 100.0f;
    public float minDistance = 50;
    public float stepDistance = 10;
    private float currentDistance;

    void Start() {
        currentDistance = maxDistance;
    }

    void LateUpdate() {
        if (!target)
            return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && currentDistance > minDistance) {
            currentDistance -= stepDistance;
        }

        //
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && currentDistance < maxDistance) {
            currentDistance += stepDistance;
        }

        transform.position = target.position + Vector3.up * 1;
        transform.position -= transform.rotation * Vector3.forward * currentDistance;
    }

    public void SetTarget(Transform t) {
        target = t;
    }

    public void SetGrayscale(bool value) {
        GetComponent<GrayscaleEffect>().enabled = value;
    }

}
