using UnityEngine;
using System.Collections;
using System;

public class ConnectMasterServerScript : MonoBehaviour {


    public string gameName = "You must change this";
    public bool allowsDedicatedServer = false;
    public GUISkin customSkin;
    public int maxUsernameLength = 20;
    public string DEFAULT_USERNAME = "Player";
    public UsernameHolder usernameHolder;

    public double serverListRefreshTime = 3.0;
    private double lastHostListRequest = -1000.0;

    private ConnectionTesterStatus connectionTestResult = ConnectionTesterStatus.Undetermined;
    private bool filterNATHosts = false;
    private bool probingPublicIP = false;
    private bool doneTesting = false;
    private double timer = 0.0;
    private bool useNat = true;

    private Rect windowRect;
    private Rect serverListRect;
    private string testMessage = "Undetermined NAT capabilities";
    private string USERNAME_INPUT_NAME = "Username text field";
    private string usernameField = "";


    void OnFailedToConnectToMasterServer(NetworkConnectionError info) {
        Debug.Log(info);
    }

    void OnFailedToConnect(NetworkConnectionError info) {
        Debug.Log(info);
        this.enabled = true;
    }

    void OnGUI() {
        GUI.skin = customSkin;
        windowRect = new Rect(Screen.width / 2 - 300, 0, 600, 100);
        float serverListHeight = 400;
        float serverListY = Screen.height - serverListHeight - 50;
        if (serverListY < 150) {
            serverListY = 150;
        }
        serverListRect = new Rect(Screen.width / 2 - Screen.width * 0.45f, serverListY, Screen.width * 0.9f, serverListHeight);
        if (Network.peerType == NetworkPeerType.Disconnected || Network.isServer) {
            windowRect = GUILayout.Window(GameConstants.WIN_ID_SERVER, windowRect, MakeWindow, "");
        }
        if (Network.peerType == NetworkPeerType.Disconnected)
            serverListRect = GUILayout.Window(GameConstants.WIN_ID_CLIENT, serverListRect, MakeClientWindow, "");
    }

    void Awake() {
        connectionTestResult = Network.TestConnection();
    }

    void Start() {
        usernameField = PlayerPrefs.GetString(GameConstants.PREFS_USERNAME);
        if (usernameField == "") {
            usernameField = DEFAULT_USERNAME;
        }
        if (allowsDedicatedServer && CommandLineParser.IsBatchMode()) {
            DoStartServer();
        }
    }

    void DoStartServer() {
        Network.InitializeServer(CommandLineParser.GetMaxPlayersAllowed(), CommandLineParser.GetServerPort(), useNat);
        MasterServer.RegisterHost(gameName, CommandLineParser.GetServerName(), CommandLineParser.GetServerComment());
    }

    void Update() {
        // If test is undetermined, keep running
        if (!doneTesting)
            TestConnection();
        if (Time.realtimeSinceStartup > lastHostListRequest + serverListRefreshTime) {
            RefreshServers();
        }
    }

    void TestConnection() {
        // Start/Poll the connection test, report the results in a label and react to the results accordingly
        connectionTestResult = Network.TestConnection();
        switch (connectionTestResult) {
            case ConnectionTesterStatus.Error:
                testMessage = "Problem determining NAT capabilities";
                doneTesting = true;
                break;

            case ConnectionTesterStatus.Undetermined:
                testMessage = "Undetermined NAT capabilities";
                doneTesting = false;
                break;

            case ConnectionTesterStatus.PublicIPIsConnectable:
                testMessage = "Directly connectable public IP address.";
                useNat = false;
                doneTesting = true;
                break;

            // This case is a bit special as we now need to check if we can 
            // circumvent the blocking by using NAT punchthrough
            case ConnectionTesterStatus.PublicIPPortBlocked:
                testMessage = "Non-connectble public IP address (port " + CommandLineParser.GetServerPort() + " blocked), running a server is impossible.";
                useNat = false;
                // If no NAT punchthrough test has been performed on this public IP, force a test
                if (!probingPublicIP) {
                    connectionTestResult = Network.TestConnectionNAT();
                    probingPublicIP = true;
                    timer = Time.time + 10;
                }
                    // NAT punchthrough test was performed but we still get blocked
                else if (Time.time > timer) {
                    probingPublicIP = false; 		// reset
                    useNat = true;
                    doneTesting = true;
                }
                break;
            case ConnectionTesterStatus.PublicIPNoServerStarted:
                testMessage = "Public IP address but server not initialized, it must be started to check server accessibility. Restart connection test when ready.";
                break;

            case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
                testMessage = "Limited NAT punchthrough capabilities. Cannot connect to all types of NAT servers.";
                useNat = true;
                doneTesting = true;
                break;

            case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
                testMessage = "Limited NAT punchthrough capabilities. Cannot connect to all types of NAT servers. Running a server is ill adviced as not everyone can connect.";
                useNat = true;
                doneTesting = true;
                break;

            case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
            case ConnectionTesterStatus.NATpunchthroughFullCone:
                testMessage = "NAT punchthrough capable. Can connect to all servers and receive connections from all clients. Enabling NAT punchthrough functionality.";
                useNat = true;
                doneTesting = true;
                break;

            default:
                testMessage = "Error in test routine, got " + connectionTestResult;
                break;
        }
        if (!testMessage.Contains("Undetermined"))
            Debug.Log(testMessage);
    }

    void RefreshServers() {
        MasterServer.RequestHostList(gameName);
        lastHostListRequest = Time.realtimeSinceStartup;
    }

    void MakeWindow(int id) {
        if (Network.peerType == NetworkPeerType.Disconnected) {
            GUILayout.Space(50);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            // Start a new server
            if (allowsDedicatedServer) {
                if (GUILayout.Button("Start Server")) {
                    DoStartServer();
                }
            }

            // Refresh hosts
            if (GUILayout.Button("Refresh available Servers")) {
                RefreshServers();
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label("Username:");
            GUI.SetNextControlName(USERNAME_INPUT_NAME);
            usernameField = GUILayout.TextField(usernameField, maxUsernameLength, GUILayout.MinWidth(150));

            GUILayout.EndHorizontal();
        } else {
            if (GUILayout.Button("Disconnect")) {
                Network.Disconnect();
                MasterServer.UnregisterHost();
            }
            GUILayout.FlexibleSpace();
        }
    }

    void MakeClientWindow(int id) {
        GUILayout.Space(25);

        HostData[] data = MasterServer.PollHostList();
        foreach (HostData element in data) {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();


            // Do not display NAT enabled games if we cannot do NAT punchthrough
            if (!(filterNATHosts && element.useNat)) {
                var connections = (element.connectedPlayers - 1) + "/" + (element.playerLimit - 1);
                GUILayout.Label(element.gameName);
                GUILayout.Space(5);
                GUILayout.Label(connections);
                GUILayout.Space(5);
                GUILayout.Label(element.comment);
                GUILayout.Space(5);
                GUILayout.FlexibleSpace();
                GUILayout.Width(100);
                if (element.connectedPlayers < element.playerLimit) {
                    if (GUILayout.Button("Join Match", "ShortButton")) {
                        RefreshServers();
                        PlayerPrefs.SetString(GameConstants.PREFS_USERNAME, usernameField);
                        // TESTINGWISE>
                        if (usernameField == "Manu") {
                            usernameField = "T[" + Time.time + "]";
                        }
                        usernameHolder.SetUsername(usernameField);
                        Network.Connect(element);
                        this.enabled = false;
                    }
                } else {
                    GUILayout.Label("Match has already started");
                }
            }
            GUILayout.EndHorizontal();
        }
    }

}