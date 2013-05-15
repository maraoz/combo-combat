using UnityEngine;
using System.Collections;
using System;
using Boomlagoon.JSON;

public class LobbyGUI : MonoBehaviour {


    public string gameName = "You must change this";
    public bool allowsDedicatedServer = false;
    public GUISkin customSkin;

    private ConnectionTesterStatus connectionTestResult = ConnectionTesterStatus.Undetermined;
    private bool filterNATHosts = false;
    private bool probingPublicIP = false;
    private bool doneTesting = false;
    private double timer = 0.0;
    private bool useNat = true;

    private Rect windowRect;
    private Rect serverListRect;
    private Rect creditsRect;
    private string testMessage = "Undetermined NAT capabilities";
    private string message = "";
    private int attempts;
    private bool waitingForOpponent = false;


    void Awake() {
        connectionTestResult = Network.TestConnection();
    }

    void Start() {
        if (allowsDedicatedServer && CommandLineParser.IsBatchMode()) {
            DoStartServer();
        }
    }

    void DoStartServer() {

        NetworkConnectionError error = NetworkConnectionError.CreateSocketOrThreadFailure;
        int port = CommandLineParser.GetServerPort();
        while (error != NetworkConnectionError.NoError) {
            error = Network.InitializeServer(CommandLineParser.GetMaxPlayersAllowed(), port, useNat);
            if (CommandLineParser.IsDynamicPort()) {
                port += 1;
            } else {
                if (error != NetworkConnectionError.NoError)
                    Debug.LogError(error);
                break;
            }
            if (port > CommandLineParser.GetServerPort() + 100) {
                break; // timeout
            }
        }
        MasterServer.RegisterHost(gameName, CommandLineParser.GetServerName(), CommandLineParser.GetServerComment());
    }

    void Update() {
        // If test is undetermined, keep running
        if (!doneTesting)
            TestConnection();
    }

    void OnGUI() {
        GUI.skin = customSkin;

        // main window
        windowRect = new Rect(50, 50, Screen.width - 100, Screen.height - 100);
        windowRect = GUILayout.Window(GameConstants.WIN_ID_SERVER, windowRect, MakeWindow, "");

        // credits button
        creditsRect = new Rect(Screen.width - 100, Screen.height - 25, 100, 25);
        GUI.Button(creditsRect, "Credits");
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.y = Screen.height - mousePosition.y;

        if (creditsRect.Contains(mousePosition)) {
            GUILayout.Window(GameConstants.WIN_ID_CREDITS, new Rect(40, 40, Screen.width - 80, Screen.height - 80), MakeCreditsWindow, "");
            GUI.BringWindowToFront(GameConstants.WIN_ID_CREDITS);
        }

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
                if (GUILayout.Button("Connect locally")) {
                    Network.Connect("localhost", CommandLineParser.GetServerPort());
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label("Playing as " + UsernameHolder.GetUsername());
            if (GUILayout.Button("Logout")) {
                Application.Quit();
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(50);
            if (GUILayout.Button("Quick Match")) {
                StartCoroutine(OnQuickPressed());
            }
            GUILayout.Box(message, GUILayout.Height(100f));
            if (waitingForOpponent == true) {
                if (GUILayout.Button("Cancel search")) {
                    waitingForOpponent = false;
                    attempts = 0;
                }
            }
        }
    }

    private IEnumerator OnQuickPressed() {
        if (waitingForOpponent) {
            yield return null;
        }
        string username = UsernameHolder.GetUsername();
        WWW www = new WWW("http://www.combocombat.com/api/search?u=" + username);
        message = "Contacting matchmaking server...";
        yield return www;
        if (www.error != "") {
            message = www.error;
            yield return null;
        }
        string text = www.text;
        JSONObject json = JSONObject.Parse(text);
        bool success = json.GetBoolean("success");
        if (success == true) {
            message = "Waiting for opponent...";
            waitingForOpponent = true;
            StartCoroutine(OnLookOpponent());
        } else {
            message = "Quick match failed: " + json.GetString("error");
        }
    }

    private IEnumerator OnLookOpponent() {
        string username = UsernameHolder.GetUsername();
        WWW www = new WWW("http://www.combocombat.com/api/check?u=" + username);
        yield return www;
        if (www.error != "") {
            message = www.error;
            yield return null;
        }
        string text = www.text;
        JSONObject json = JSONObject.Parse(text);
        bool success = json.GetBoolean("success");
        if (success == true) {
            string host = json.GetString("host");
            if (host == null) {
                attempts += 1;
                message = "Waiting for opponent... " + attempts;
                if (waitingForOpponent) {
                    yield return new WaitForSeconds(1);
                    StartCoroutine(OnLookOpponent());
                } else {
                    message = "Waiting cancelled.";
                }
            } else {
                message = "Opponent found! Please connect to " + host;
                attempts = 0;
            }
        } else {
            message = "Wait for opponent failed: " + json.GetString("error");
        }
    }

    void MakeClientWindow(int id) {
        GUILayout.Space(25);

        HostData[] data = MasterServer.PollHostList();
        //data = NoInternetVersion();
        foreach (HostData element in data) {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();


            // Do not display NAT enabled games if we cannot do NAT punchthrough
            if (!(filterNATHosts && element.useNat)) {
                var connections = "(" + (element.connectedPlayers - 1) + "/" + (element.playerLimit - 1) + ")";
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

    private HostData[] NoInternetVersion() {
        HostData[] data;
        data = new HostData[1];
        data[0] = new HostData();
        data[0].ip = new string[1];
        data[0].ip[0] = "localhost";
        data[0].port = 34200;
        data[0].connectedPlayers = 0;
        data[0].playerLimit = 32;
        data[0].guid = "test";
        data[0].passwordProtected = false;
        data[0].useNat = false;
        return data;
    }

    void MakeCreditsWindow(int id) {
        GUILayout.Space(35);
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Art", GUI.skin.GetStyle("ShortLabel"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label("Mariano Merchante");
        GUILayout.Label("Pablo Marseillan");
        GUILayout.Label("José Diaz");
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Game Design", GUI.skin.GetStyle("ShortLabel"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label("Tomás Migone");
        GUILayout.Label("Agustin Marseillan");
        GUILayout.Label("Francisco Tavella");
        GUILayout.EndVertical();


        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Programming", GUI.skin.GetStyle("ShortLabel"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label("Manuel Aráoz");
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Sound FX", GUI.skin.GetStyle("ShortLabel"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label("Juan Pablo Arnaudo");
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Business", GUI.skin.GetStyle("ShortLabel"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label("Valentin Fezza");
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Sound Acting", GUI.skin.GetStyle("ShortLabel"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label("Victoria Nasiff");
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();



        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Creative Commons Attribution", GUI.skin.GetStyle("ShortLabel"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label("Kevin Mac Leod, ZeSoundResearchInc., sandyrb, Goup_1, stijn, " +
            "Grant Evans, et_, broke for free, jahzzar, ellywu2, Lavoura, " +
            "Colin Johnco, eleazzaar, wildweasel, Soughtaftersounds");
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();



        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Special Thanks", GUI.skin.GetStyle("ShortLabel"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label("Vicente Macellari, Miguel De Elias, " +
            "Luciano di Lorenzi, Alejandro Park, Patricio Borghi, Ezequiel Boehler, " +
            "Carola Serra, Fernando Aráoz, Pedro Aráoz, Matias Barba, " +
            "Kevin Miyashiro, Gee Lee Hyun, Joaquin Carrascosa, Gonzalo Nicolás, " +
            "Matias Colotto, Francisco \"Kalith\" Marienhoff, Ignacio \"Ragnar\" Llerena, " +
            "Alberto \"Inglés\" Iarussi, Juani \"Reicko\" Gallo, Nico \"Waikita\" Galli, " +
            "Barklight, Ian \"Nosfe\" Flaker");
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();



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
        if (!testMessage.Contains("Undetermined")) {
            //Debug.Log(testMessage);
        }
    }


    // network events
    void OnFailedToConnectToMasterServer(NetworkConnectionError info) {
        Debug.Log(info);
    }

    void OnFailedToConnect(NetworkConnectionError info) {
        Debug.Log(info);
        this.enabled = true;
    }

    void OnConnectedToServer() {
        Network.SetSendingEnabled(0, false);
        Network.isMessageQueueRunning = false;
        LoadArenaLevel();
    }

    void OnServerInitialized() {
        LoadArenaLevel();
        Debug.Log("Server initialized and ready");
    }

    void LoadArenaLevel() {
        Network.SetLevelPrefix(GameConstants.LEVEL_PREFIX_ARENA);
        Application.LoadLevel(GameConstants.LEVEL_ARENA);
    }

}