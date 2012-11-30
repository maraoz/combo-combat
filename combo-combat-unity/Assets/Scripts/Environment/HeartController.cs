using UnityEngine;
using System.Collections;

public class HeartController : MonoBehaviour {

    public float rotationAmount = 5.0f;
    public float healing = 15;

    void Update() {
        transform.Rotate(new Vector3(0, rotationAmount, 0));
    }

    internal void Destroy() {
        Network.RemoveRPCs(networkView.viewID);
        Network.Destroy(gameObject);
    }


}
