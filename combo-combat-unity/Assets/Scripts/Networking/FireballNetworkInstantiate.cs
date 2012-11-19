using UnityEngine;
using System.Collections;

public class FireballNetworkInstantiate : MonoBehaviour {
    void OnNetworkInstantiate(NetworkMessageInfo info) {
        //if (Network.isServer) {
            Network.RemoveRPCs(info.sender, GameConstants.FIREBALL_GROUP);
        //}
    }
}
