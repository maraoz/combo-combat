using UnityEngine;
using System.Collections;

public class ConfigurationHandler : PersistentSingleton {

    public Texture2D configTexture;
    public float pad;

    private float width, height;
    private bool showConfig = false;

    public float volume = 1f;
    public int windowWidth = 150;
    public int windowHeight = 300;

    override internal void Awake() {
        base.Awake();
        volume = PlayerPrefs.GetFloat(GameConstants.PREFS_VOLUME, 1f);
        DoSetVolume(volume);
        width = configTexture.width / 2;
        height = configTexture.height / 2;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F10)) {
            ToggleConfig();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            showConfig = false;
        }
    }

    void OnGUI() {
        GUI.skin = GUISkinProvider.GetSkin();
        if (GUI.Button(new Rect(Screen.width - pad - width, 2*pad+height , width, height), configTexture, "Spell Icon")) {
            ToggleConfig();
        }

        if (showConfig) {
            GUILayout.Window(GameConstants.WIN_ID_CONFIG, new Rect(Screen.width / 2 - windowWidth/2, Screen.height / 2 - windowHeight/2, windowWidth, windowHeight), MakeCreditsWindow, "");
        }
    }

    void MakeCreditsWindow(int id) {
        GUILayout.Space(35);
        GUILayout.BeginVertical();
        GUILayout.Label("Configuration");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Volume", GUILayout.MaxWidth(80));
        volume = GUILayout.HorizontalSlider(volume, 0, 1);
        DoSetVolume(volume);
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Save")) {
            PlayerPrefs.SetFloat(GameConstants.PREFS_VOLUME, volume);
            ToggleConfig();
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel")) {
            volume = PlayerPrefs.GetFloat(GameConstants.PREFS_VOLUME, 1);
            DoSetVolume(volume);
            ToggleConfig();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void DoSetVolume(float volume) {
        AudioListener.volume = volume;
    }

    private void ToggleConfig() {
        showConfig = !showConfig;
    }

}
