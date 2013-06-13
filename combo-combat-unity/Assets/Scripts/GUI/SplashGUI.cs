using UnityEngine;
using System.Collections;
using System;
using Boomlagoon.JSON;

public class SplashGUI : MonoBehaviour {

    public string gameName = "You must change this";
    public bool allowsDedicatedServer = true;

    private ConnectionTesterStatus connectionTestResult = ConnectionTesterStatus.Undetermined;
    private string testMessage = "Undetermined NAT capabilities";
    private bool probingPublicIP = false;
    private bool doneTesting = false;
    private double timer = 0.0;
    private bool useNat = true;

    public Texture logo;
    public string DEFAULT_USERNAME = "Player";
    public int maxUsernameLength = 25;
    private string USERNAME_INPUT_NAME = "Username text field";
    private string PASSWORD_FIELD_NAME = "Password field";
    private string username;
    private string password;
    private string message;

    void Update() {
        // If test is undetermined, keep running
        if (!doneTesting)
            TestConnection();
    }
    void Awake() {
        connectionTestResult = Network.TestConnection();

        password = "";
        username = PlayerPrefs.GetString(GameConstants.PREFS_USERNAME);
        if (username == "") {
            username = DEFAULT_USERNAME;
        }

        if (CommandLineParser.IsBatchMode()) {
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

    void OnGUI() {
        int sWidth = Screen.width;
        int sHeight = Screen.height;
        int logoSize = Screen.width / 3;
        int formWidth = sWidth / 2;
        int formHeight = 200;

        GUI.Label(new Rect(Screen.width / 2 - logoSize / 2, Screen.height / 2 - logoSize * 2 / 3, logoSize, logoSize), logo);
        if (allowsDedicatedServer) {
            if (GUILayout.Button("Start Server")) {
                DoStartServer();
            }
        }
        GUILayout.BeginArea(new Rect((sWidth - formWidth) / 2, (sHeight - formHeight), formWidth, formHeight));
        GUILayout.BeginHorizontal();
        GUILayout.Button("Username: ");
        GUI.SetNextControlName(USERNAME_INPUT_NAME);
        username = GUILayout.TextField(username, maxUsernameLength, GUILayout.Width(2 * formWidth / 3));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Button("Password: ");
        GUI.SetNextControlName(PASSWORD_FIELD_NAME);
        password = GUILayout.PasswordField(password, '●', 25, GUILayout.Width(2 * formWidth / 3));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Login")) {
            StartCoroutine(OnLoginPressed());
        }
        if (GUILayout.Button("Register")) {
            StartCoroutine(OnRegisterPressed());
        }
        GUILayout.EndHorizontal();
        GUILayout.Box(message, GUILayout.Height(100f));
        GUILayout.EndArea();

        if (Event.current.type == EventType.keyUp && Event.current.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl().Equals(PASSWORD_FIELD_NAME)) {
            StartCoroutine(OnLoginPressed());
        }
    }

    private IEnumerator OnLoginPressed() {
        WWW www = new WWW("http://www.combocombat.com/api/login?u=" + username + "&p=" + password);
        message = "Loading...";
        yield return www;
        if (www.error != "") {
            message = www.error;
            yield return null;
        }
        string text = www.text;
        JSONObject json = JSONObject.Parse(text);
        bool success = json.GetBoolean("success");
        if (success == true) {
            PlayerPrefs.SetString(GameConstants.PREFS_USERNAME, username);
            if (username == "Manu") {
                username = "T[" + Time.time + "]";
            }
            UsernameHolder.SetUsername(username);
            message = "Login successful! Please wait...";

            GoToLobby();
        } else {
            message = "Login failed: " + json.GetString("error");
        }
    }


    private IEnumerator OnRegisterPressed() {
        WWW www = new WWW("http://www.combocombat.com/api/register?u=" + username + "&p=" + password);
        message = "Loading...";
        yield return www;
        string text = www.text;
        JSONObject json = JSONObject.Parse(text);
        bool success = json.GetBoolean("success");
        if (success == true) {
            message = "Successfully registered user " + username + ". You can now login to play.";
            password = "";
        } else {
            message = "Registration failed: " + json.GetString("error");
        }
    }
    private void GoToLobby() {
        Network.SetLevelPrefix(GameConstants.LEVEL_PREFIX_LOBBY);
        Application.LoadLevel(GameConstants.LEVEL_LOBBY);
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

    void OnServerInitialized() {
        Network.SetLevelPrefix(GameConstants.LEVEL_PREFIX_ARENA);
        Application.LoadLevel(GameConstants.LEVEL_ARENA);
        Debug.Log("Server initialized and ready");
    }

    void OnFailedToConnectToMasterServer(NetworkConnectionError info) {
        Debug.Log(info);
    }

}