using UnityEngine;
using System.Collections;

public class MageNetworkInstantiate : MonoBehaviour {

    private MessageSystem messages;

    void Awake() {
        messages = GameObject.Find("MessageSystem").GetComponent<MessageSystem>();
    }

    void OnNetworkInstantiate(NetworkMessageInfo msg) {
        messages.AddSystemMessage("New player joined game.", false);
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