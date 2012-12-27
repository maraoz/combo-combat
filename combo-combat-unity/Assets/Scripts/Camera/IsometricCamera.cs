using UnityEngine;
using System.Collections;

public class IsometricCamera : MonoBehaviour {

    Transform target;

    public float distance = 100.0f;

    public int maxScale = 12;
    public int minScale = 5;

    void Start() {
        camera.orthographicSize = maxScale;
    }

    void LateUpdate() {
        if (!target)
            return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && camera.orthographicSize > minScale) {
            camera.orthographicSize -= 1;
        }

        //
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && camera.orthographicSize < maxScale) {
            camera.orthographicSize += 1;
        }

        //default zoom
        if (Input.GetKeyDown(KeyCode.Mouse2)) {
            camera.orthographicSize = maxScale;
        }

        transform.position = target.position + Vector3.up * 1;
        transform.position -= transform.rotation * Vector3.forward * distance;
    }

    public void SetTarget(Transform t) {
        target = t;
    }

    public void SetGrayscale(bool value) {
        GetComponent<GrayscaleEffect>().enabled = value;
    }

}
