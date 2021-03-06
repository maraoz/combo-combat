using UnityEngine;
using System.Collections;

public class PingProbe : PersistentSingleton {

    public float timeBetweenPings = 1;
    public bool showLatency;

    private bool pingSent;
    private float pingSentTimestamp;
    private float latency;

    override internal void Awake() {
        base.Awake();
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
        if (showLatency) {
            float width = 130;
            float height = 25;
            GUI.Label(new Rect(Screen.width - width, Screen.height - height, width, height), "Latency: " + ((int) latency) + " ms");
            if (Network.isServer) {
                GUILayout.BeginVertical();
                GUILayout.Space(100);
                GUILayout.Label("Player ping values");
                int i = 0;
                while (i < Network.connections.Length) {
                    GUILayout.Label("Player " + Network.connections[i] + " - " + Network.GetAveragePing(Network.connections[i]) + " ms");
                    i++;
                }
            }

        }
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
