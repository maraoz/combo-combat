using UnityEngine;
using System.Collections;

public class HeartController : MonoBehaviour {

    public float healing = 15;

    internal void DoDestroy() {
        Network.Destroy(gameObject);
        Network.RemoveRPCs(networkView.viewID);
    }


}
