using UnityEngine;
using System.Collections;
using System;
using Boomlagoon.JSON;

public class LobbyGUI : MonoBehaviour {


    public bool allowsLocalConnection = true;
    public GUISkin customSkin;


    private Rect windowRect;
    private Rect serverListRect;
    private Rect creditsRect;
    private string message = "";
    private int attempts;
    private bool waitingForOpponent = false;


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
            if (allowsLocalConnection) {
                if (GUILayout.Button("Connect locally")) {
                    Network.Connect("localhost", CommandLineParser.GetServerPort());
                }

                GUILayout.FlexibleSpace();
                GUILayout.Label("Playing as " + UsernameHolder.GetUsername());
                if (GUILayout.Button("Logout")) {
                    Application.Quit();
                }

                GUILayout.EndHorizontal();
                GUILayout.Space(50);
                if (!waitingForOpponent && GUILayout.Button("Quick Match")) {
                    StartCoroutine(OnQuickPressed());
                }
                if (waitingForOpponent == true) {
                    if (GUILayout.Button("Cancel search")) {
                        waitingForOpponent = false;
                        attempts = 0;
                    }
                }
                GUILayout.Box(message, GUILayout.Height(100f));
            }
        }
    }

    private IEnumerator OnQuickPressed() {
        if (waitingForOpponent) {
            yield return null;
        }
        waitingForOpponent = true;
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
                message = "Opponent found! Connecting to " + host;
                ConnectToServer(host);
                attempts = 0;
            }
        } else {
            message = "Wait for opponent failed: " + json.GetString("error");
        }
    }

    void ConnectToServer(string host_ip) {
        String[] split = host_ip.Split(':');
        String ip = split[0];
        int port = int.Parse(split[1]);

        Network.Connect(ip, port);
        this.enabled = false;
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


    // network events
    void OnFailedToConnect(NetworkConnectionError info) {
        Debug.Log(info);
        this.enabled = true;
    }

    void OnConnectedToServer() {
        Network.SetSendingEnabled(0, false);
        Network.isMessageQueueRunning = false;
        Network.SetLevelPrefix(GameConstants.LEVEL_PREFIX_ARENA);
        Application.LoadLevel(GameConstants.LEVEL_ARENA);
    }

}