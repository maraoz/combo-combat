using UnityEngine;
using System.Collections;

public class CharacterNetworkInstantiate : MonoBehaviour {
    void OnNetworkInstantiate(NetworkMessageInfo msg) {
        if (networkView.isMine) {
            Camera.main.SendMessage("SetTarget", transform);
        } else {
            name += "Remote";
            GetComponent<ClickPlayerMovementScript>().enabled = false;
            GetComponent<CharacterSimpleAnimation>().enabled = false;
            GetComponent<NetworkInterpolatedTransform>().enabled = true;
        }
    }

}
