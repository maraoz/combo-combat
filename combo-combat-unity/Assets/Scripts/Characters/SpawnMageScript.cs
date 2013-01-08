using UnityEngine;
using System.Collections;

public class SpawnMageScript : MonoBehaviour {

    public GameObject playerPrefab;
    private MessageSystem messages;
    private int playerCount = -1;
    private NetworkPlayer[] players;
    private float countdownStart = 0;
    public int secondsOfCountdown = 5;
    private int secondsRemaining;
    private bool isFreeMode;

    private AudioSource beepSound;
    private AudioSource drumSound;
    private AudioSource gongSound;

    void Start() {
        AudioSource[] aSources = GetComponents<AudioSource>();
        beepSound = aSources[0];
        drumSound = aSources[1];
        gongSound = aSources[2];
        ResetGameTimer();

        messages = GameObject.Find("MessageSystem").GetComponent<MessageSystem>();

        Network.isMessageQueueRunning = true;
        Network.SetSendingEnabled(0, true);

        if (Network.isServer) {
            playerCount = 0;
            isFreeMode = CommandLineParser.IsFreeMode();
            SetPlayerMaxCount(Network.maxConnections);
        }
        if (Network.isClient) {
            string username = UsernameHolder.MyUsername();
            networkView.RPC("SpawnMage", RPCMode.Server, username); // need NetworkMessageInfo
        }
    }

    void ResetGameTimer() {
        secondsRemaining = secondsOfCountdown;
        countdownStart = 0;
    }

    void Update() {
        if (countdownStart != 0) {
            if (playerCount < players.Length) {
                messages.AddSystemMessageSelf("Round start cancelled");
                ResetGameTimer();
                return;
            }
            float now = Time.time;
            float delta = now - countdownStart;
            if (delta > secondsOfCountdown - secondsRemaining) {
                drumSound.Play();
                messages.AddSystemMessageSelf("Round starting in " + secondsRemaining);
                secondsRemaining -= 1;
            }
            if (secondsRemaining == 0) {
                ResetGameTimer();
                messages.AddSystemMessageSelf("Round started!");
                GameObject[] mages = GameObject.FindGameObjectsWithTag(GameConstants.MAGE_TAG);
                foreach (GameObject mage in mages) {
                    gongSound.Play();
                    mage.GetComponent<Mage>().StartRound();
                }
            }
        }
    }

    void OnGUI() {
        if (!isFreeMode && playerCount != -1 && countdownStart == 0 && playerCount < players.Length) {
            GUIStyle signStyle = new GUIStyle();
            signStyle.fontSize = 40;
            signStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.BeginVertical();
            GUILayout.Space(200);
            GUILayout.Label("Waiting for other players to join.", signStyle, GUILayout.Width(Screen.width));
            int delta = (players.Length - playerCount);
            string s = delta == 1 ? "" : "s";
            GUILayout.Label(delta + " more player" + s + " needed", signStyle, GUILayout.Width(Screen.width));
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
    }

    [RPC]
    void SpawnMage(string username, NetworkMessageInfo info) {
        if (networkView.Server("SpawnMage", username)) {
            if (playerCount >= Network.maxConnections) {
                return;
            }
            for (int i = 0; i < players.Length; i++) {
                if (players[i] == GameConstants.NO_PLAYER) {
                    messages.AddSystemMessageBroadcast(username + " connected.");
                    players[i] = info.sender;
                    DoSpawnMage(i, username, info);
                    if (!isFreeMode) {
                        SetPlayerCurrentCount(playerCount + 1);
                    }
                    break;
                }
            }
        }
    }

    void DoSpawnMage(int i, string username, NetworkMessageInfo info) {
        float omega = ((float) i) / Network.maxConnections * 2 * Mathf.PI;
        float r = 10;
        Vector3 pos = transform.position + r * Vector3.left * Mathf.Cos(omega) + r * Vector3.forward * Mathf.Sin(omega);
        Quaternion quat = Quaternion.LookRotation(transform.position - pos);
        GameObject mageObject = Network.Instantiate(playerPrefab, pos, quat, GameConstants.MAGE_GROUP) as GameObject;
        mageObject.transform.LookAt(transform);
        MageLifeController life = mageObject.GetComponent<MageLifeController>();
        Mage mage = mageObject.GetComponent<Mage>();
        UserInputController input = mageObject.GetComponent<UserInputController>();
        input.ServerInit();
        life.SetUsername(username);
        mage.SetPlayer(info.sender);
        life.SetSpawnPosition(pos);
        if (isFreeMode) {
            mage.StartRound();
        }
    }

    // called on client
    void OnDisconnectedFromServer() {
        messages.AddSystemMessageSelf("Connection to server lost :( Please refresh page.");

        // TODO: return to lobby
    }

    // called on server
    void OnPlayerConnected(NetworkPlayer player) {
        PlayConnectSound();
    }

    // called on server
    void OnPlayerDisconnected(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        for (int i = 0; i < players.Length; i++) {
            if (players[i] != GameConstants.NO_PLAYER && players[i].Equals(player)) {
                players[i] = GameConstants.NO_PLAYER;
                SetPlayerCurrentCount(playerCount - 1);
                break;
            }
        }
    }

    [RPC]
    void SetPlayerMaxCount(int n) {
        networkView.Clients("SetPlayerMaxCount", n);
        players = new NetworkPlayer[n];
    }

    [RPC]
    void SetPlayerCurrentCount(int newPlayerCount) {
        networkView.ClientsUnbuffered("SetPlayerCurrentCount", newPlayerCount);
        playerCount = newPlayerCount;
        if (playerCount == players.Length && !isFreeMode) {
            countdownStart = Time.time;
        }
    }

    [RPC]
    void PlayConnectSound() {
        networkView.ClientsUnbuffered("PlayConnectSound");
        beepSound.Play();
    }

}