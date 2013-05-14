using UnityEngine;
using System.Collections;
using System;
using Boomlagoon.JSON;

public class SplashGUI : MonoBehaviour {

    public Texture logo;
    public string DEFAULT_USERNAME = "Player";
    public int maxUsernameLength = 25;
    private string USERNAME_INPUT_NAME = "Username text field";
    private string PASSWORD_FIELD_NAME = "Password field";
    private string username;
    private string password;
    private string message;

    void Awake() {

        password = "";
        username = PlayerPrefs.GetString(GameConstants.PREFS_USERNAME);
        if (username == "") {
            username = DEFAULT_USERNAME;
        }

        if (CommandLineParser.IsBatchMode()) {
            GoToLobby();
        }

    }

    void OnGUI() {
        int sWidth = Screen.width;
        int sHeight = Screen.height;
        int logoSize = Screen.width / 3;
        int formWidth = sWidth / 2;
        int formHeight = 200;

        GUI.Label(new Rect(Screen.width / 2 - logoSize / 2, Screen.height / 2 - logoSize * 2 / 3, logoSize, logoSize), logo);

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

}