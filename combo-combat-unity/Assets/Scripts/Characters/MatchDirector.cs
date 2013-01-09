using UnityEngine;
using System.Collections;

public class MatchDirector : MonoBehaviour {

    public GameObject playerPrefab;

    private MessageSystem messages;
    private int playerCount = -1;
    private int deadPlayerCount = 0;
    private NetworkPlayer[] players;
    private float countdownStart = 0;
    public int secondsOfCountdown = 5;
    private int secondsRemaining;
    private bool isFreeMode;
    private static int WIN_MESSAGE = 1;
    private static int LOSE_MESSAGE = 2;
    private int showWinMessage = 0;
    public float endMatchWait = 10;
    private float endMatchTime;

    private GUIStyle signStyle;
    private AudioSource beepSound;
    private AudioSource drumSound;
    private AudioSource gongSound;

    void Start() {
        AudioSource[] aSources = GetComponents<AudioSource>();
        beepSound = aSources[0];
        drumSound = aSources[1];
        gongSound = aSources[2];
        signStyle = new GUIStyle();
        signStyle.fontSize = 40;
        signStyle.alignment = TextAnchor.MiddleCenter;

        Network.SetSendingEnabled(0, true);
        Network.isMessageQueueRunning = true;

        messages = GameObject.Find("MessageSystem").GetComponent<MessageSystem>();

        ResetGameTimer();

        if (Network.isServer) {
            deadPlayerCount = 0;
            playerCount = 0;
            endMatchTime = 0;
            SetMatchMode(Network.maxConnections, CommandLineParser.IsFreeMode());
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
                GameObject[] mages = GameObject.FindGameObjectsWithTag(GameConstants.TAG_MAGE);
                foreach (GameObject mage in mages) {
                    gongSound.Play();
                    mage.GetComponent<Mage>().StartRound();
                }
            }
        }
        if (endMatchTime != 0) {
            float delta = Time.time - endMatchTime;
            if (delta > endMatchWait) {
                // send players back to lobby
                EndMatch();
            }
        }
    }

    void OnGUI() {
        if (showWinMessage != 0 || (!isFreeMode && playerCount != -1 && countdownStart == 0 && playerCount < players.Length)) {
            GUILayout.BeginVertical();
            GUILayout.Space(200);
            if (showWinMessage == 0) {
                GUILayout.Label("Waiting for other players to join.", signStyle, GUILayout.Width(Screen.width));
                int delta = (players.Length - playerCount);
                GUILayout.Label(delta + " more player" + (delta == 1 ? "" : "s") + " needed", signStyle, GUILayout.Width(Screen.width));
            } else {
                string message = showWinMessage == WIN_MESSAGE ? "won" : "lost";
                GUILayout.Label("You have " + message + " the match!", signStyle, GUILayout.Width(Screen.width));
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
    }

    [RPC]
    void SpawnMage(string username, NetworkMessageInfo info) {
        if (networkView.Server("SpawnMage", username)) {
            if (playerCount >= players.Length) {
                return;
            }
            for (int i = 0; i < players.Length; i++) {
                if (players[i] == GameConstants.NO_PLAYER) {
                    messages.AddSystemMessageBroadcast(username + " connected.");
                    players[i] = info.sender;
                    DoSpawnMage(i, username, info);
                    if (!isFreeMode) {
                        SetPlayerCurrentCount(playerCount + 1);
                        messages.AddSystemMessageTo(info.sender, "Welcome to match mode. You have " + 3 + " lives. Last mage standing wins the match.");
                    } else {
                        messages.AddSystemMessageTo(info.sender, "Welcome to free mode. You have infinite lives. You can't win in this game mode.");
                    }
                    break;
                }
            }
        }
    }

    void DoSpawnMage(int i, string username, NetworkMessageInfo info) {
        // server side
        float omega = ((float) i) / Network.maxConnections * 2 * Mathf.PI;
        float r = 10;
        Vector3 pos = transform.position + r * Vector3.left * Mathf.Cos(omega) + r * Vector3.forward * Mathf.Sin(omega);
        Quaternion quat = Quaternion.LookRotation(transform.position - pos);
        GameObject mageObject = Network.Instantiate(playerPrefab, pos, quat, GameConstants.GROUP_MAGE) as GameObject;
        mageObject.transform.LookAt(transform);
        MageLifeController life = mageObject.GetComponent<MageLifeController>();
        Mage mage = mageObject.GetComponent<Mage>();
        UserInputController input = mageObject.GetComponent<UserInputController>();
        input.ServerInit();
        mage.SetPlayer(info.sender);
        life.SetFreeMode(isFreeMode);
        life.SetUsername(username);
        life.SetSpawnPosition(pos);
        life.SetMatchDirector(this);
        if (isFreeMode) {
            mage.StartRound();
        }
    }

    // called on server
    void OnPlayerConnected(NetworkPlayer player) {
        PlayConnectSound();
    }

    // called on server
    void OnDisconnectedFromServer(NetworkDisconnection info) {
        if (info == NetworkDisconnection.LostConnection) {
            messages.AddSystemMessageSelf("Connection to server lost :( You will be redirected to the lobby.");
        } else {
            Network.SetLevelPrefix(GameConstants.LEVEL_PREFIX_LOBBY);
            Application.LoadLevel(GameConstants.LEVEL_LOBBY);
        }

    }

    // called on server
    void OnPlayerDisconnected(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        for (int i = 0; i < players.Length; i++) {
            if (players[i] != GameConstants.NO_PLAYER && players[i].Equals(player)) {
                players[i] = GameConstants.NO_PLAYER;
                if (!isFreeMode) {
                    SetPlayerCurrentCount(playerCount - 1);
                }
                break;
            }
        }
    }

    [RPC]
    void SetMatchMode(int n, bool freeMode) {
        networkView.Clients("SetMatchMode", n, freeMode);
        players = new NetworkPlayer[n];
        isFreeMode = freeMode;
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


    internal void OnPlayerLost() {
        deadPlayerCount += 1;
        if (deadPlayerCount == playerCount - 1) {
            // match ends
            Mage[] mages = FindObjectsOfType(typeof(Mage)) as Mage[];
            foreach (Mage mage in mages) {
                NetworkPlayer player = mage.GetPlayer();
                networkView.RPC("WinMessage", player, !mage.IsDying());
            }

            endMatchTime = Time.time;
        }
    }

    [RPC]
    void WinMessage(bool won) {
        showWinMessage = won ? WIN_MESSAGE : LOSE_MESSAGE;
    }

    void EndMatch() {
        if (Network.isServer) {
            foreach (NetworkPlayer connection in Network.connections) {
                Network.CloseConnection(connection, true);
            }
            ResetGameTimer();
            deadPlayerCount = 0;
            endMatchTime = 0;
        }
    }

}