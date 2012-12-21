using UnityEngine;
using System.Collections;

public class FullscreenHandler : MonoBehaviour {

    public Texture2D fullscreenOnTexture;
    public Texture2D fullscreenOffTexture;
    public float pad;
    private float width, height;
    private Resolution minimizedResolution;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        width = fullscreenOnTexture.width / 2;
        height = fullscreenOnTexture.height / 2;
        minimizedResolution = new Resolution();
        minimizedResolution.width = Screen.width;
        minimizedResolution.height = Screen.height;
    }

    void FullscreenOn() {
        if (Screen.fullScreen) {
            return;
        }
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
    }

    void FullscreenOff() {
        if (!Screen.fullScreen) {
            return;
        }
        Screen.SetResolution(minimizedResolution.width, minimizedResolution.height, false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            FullscreenOn();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            FullscreenOff();
        }
    }

    void OnGUI() {
        GUI.skin = GUISkinProvider.GetSkin();
        if (GUI.Button(new Rect(Screen.width - pad - width, pad, width, height), Screen.fullScreen ? fullscreenOffTexture : fullscreenOnTexture, "Spell Icon")) {
            if (Screen.fullScreen) {
                FullscreenOff();
            } else {
                FullscreenOn();
            }
        }
    }

}
