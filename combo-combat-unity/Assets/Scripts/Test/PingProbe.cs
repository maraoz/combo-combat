using UnityEngine;
using System.Collections;

public class PingProbe : MonoBehaviour {

    public float timeBetweenPings = 1;

    private bool pingSent;
    private float pingSentTimestamp;
    private float latency;
    void Start() {
        DontDestroyOnLoad(gameObject);
        pingSent = false;
        pingSentTimestamp = 0;
        latency = 1;
    }

    void Update() {
        if (Network.isClient) {
            if (!pingSent) {
                float timeSinceLastPing = Time.time - pingSentTimestamp;
                if (timeSinceLastPing > timeBetweenPings) {
                    pingSent = true;
                    pingSentTimestamp = Time.time;
                    networkView.RPC("Ping", RPCMode.Server);
                }
            }
        }

    }

    void OnGUI() {
        float width = 130;
        float height = 25;
        GUI.Label(new Rect(Screen.width - width, Screen.height - height, width, height), "Latency: " + ((int)latency) + " ms");
    }

    [RPC]
    void Ping(NetworkMessageInfo info) {
        networkView.RPC("Pong", info.sender);
    }

    [RPC]
    void Pong() {
        latency = (Time.time - pingSentTimestamp) * 1000;
        pingSent = false;
    }
}
