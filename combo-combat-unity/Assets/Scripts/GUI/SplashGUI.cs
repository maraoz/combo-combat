using UnityEngine;
using System.Collections;
using System;
using Boomlagoon.JSON;

public class SplashGUI : MonoBehaviour {

    public Texture logo;
    public string DEFAULT_USERNAME = "Player";
    public int maxUsernameLength = 25;
    private string USERNAME_INPUT_NAME = "Username text field";
    private string username;
    private string password;

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
        int formHeight = 100;

        GUI.Label(new Rect(Screen.width / 2 - logoSize / 2, Screen.height / 2 - logoSize * 2 / 3, logoSize, logoSize), logo);

        GUILayout.BeginArea(new Rect((sWidth - formWidth) / 2, (sHeight - formHeight), formWidth, formHeight));
        GUILayout.BeginHorizontal();
        GUILayout.Button("Username: ");
        GUI.SetNextControlName(USERNAME_INPUT_NAME);
        username = GUILayout.TextField(username, maxUsernameLength, GUILayout.Width(2 * formWidth / 3));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Button("Password: ");
        password = GUILayout.PasswordField(password, '●', 25, GUILayout.Width(2 * formWidth / 3));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Login")) {
            WWW www = new WWW("http://www.combocombat.com/api/login?u=maraoz&p=123456");
            yield return www;
            string text = www.text;
            JSONObject json = JSONObject.Parse(text);
            bool success = json.GetBoolean("success");
            if (success == true) {
                OnSuccessfulLogin();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void OnSuccessfulLogin() {
        PlayerPrefs.SetString(GameConstants.PREFS_USERNAME, username);
        // TESTINGWISE>
        if (username == "Manu") {
            username = "T[" + Time.time + "]";
        }
        UsernameHolder.SetUsername(username);

        GoToLobby();

    }

    private void GoToLobby() {
        Network.SetLevelPrefix(GameConstants.LEVEL_PREFIX_LOBBY);
        Application.LoadLevel(GameConstants.LEVEL_LOBBY);
    }

}