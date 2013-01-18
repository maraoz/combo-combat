using UnityEngine;
using System.Collections;

public class FullscreenHandler : PersistentSingleton {

    public Texture2D fullscreenOnTexture;
    public Texture2D fullscreenOnOverTexture;
    public Texture2D fullscreenOffTexture;
    public Texture2D fullscreenOffOverTexture;
    public float pad;
    private float width, height;
    private Resolution minimizedResolution;
    private Rect buttonRect;
    private bool mouseOver;

    override internal void Awake() {
        base.Awake();
        width = fullscreenOnTexture.width / 4;
        height = fullscreenOnTexture.height / 4;
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
        Vector2 mousePos = Input.mousePosition;
        mousePos.y = Screen.height - mousePos.y;
        mouseOver = buttonRect.Contains(mousePos);
    }

    void OnGUI() {
        GUI.skin = GUISkinProvider.GetSkin();
        buttonRect = new Rect(Screen.width - pad - width, pad, width, height);
        Texture2D texture = Screen.fullScreen ? (mouseOver ? fullscreenOffOverTexture : fullscreenOffTexture) : (mouseOver ? fullscreenOnOverTexture : fullscreenOnTexture);
        if (GUI.Button(buttonRect, texture, "Spell Icon")) {
            if (Screen.fullScreen) {
                FullscreenOff();
            } else {
                FullscreenOn();
            }
        }
    }

}
