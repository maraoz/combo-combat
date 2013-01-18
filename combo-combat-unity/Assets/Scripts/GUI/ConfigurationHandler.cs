using UnityEngine;
using System.Collections;

public class ConfigurationHandler : PersistentSingleton {

    public Texture2D configTexture;
    public Texture2D configTextureOver;
    public float pad;

    private float width, height;
    private bool showConfig = false;

    public float volume = 1f;
    public int windowWidth = 150;
    public int windowHeight = 300;
    public float defaultVolumeLevel = 0.5f;
    private Rect buttonRect;
    private bool mouseOver;

    override internal void Awake() {
        base.Awake();
        volume = PlayerPrefs.GetFloat(GameConstants.PREFS_VOLUME, defaultVolumeLevel);
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
        Vector2 mousePos = Input.mousePosition;
        mousePos.y = Screen.height - mousePos.y;
        mouseOver = buttonRect.Contains(mousePos);
    }

    void OnGUI() {
        GUI.skin = GUISkinProvider.GetSkin();
        buttonRect = new Rect(Screen.width - 10 - width, Screen.height - pad - height, width, height);
        if (GUI.Button(buttonRect, mouseOver ? configTextureOver : configTexture, "Spell Icon")) {
            ToggleConfig();
        }

        if (showConfig) {
            GUILayout.Window(GameConstants.WIN_ID_CONFIG, new Rect(Screen.width / 2 - windowWidth / 2, Screen.height / 2 - windowHeight / 2, windowWidth, windowHeight), MakeCreditsWindow, "");
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
            volume = PlayerPrefs.GetFloat(GameConstants.PREFS_VOLUME, defaultVolumeLevel);
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
