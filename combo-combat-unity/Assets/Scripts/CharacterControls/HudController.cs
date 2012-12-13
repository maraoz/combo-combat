using UnityEngine;
using System.Collections;

public class HudController : MonoBehaviour {

    public Texture2D spellBar;

    private Rect spellBarRect;

    void Awake() {
        int width = spellBar.width / 4;
        int height = spellBar.height / 4;
        spellBarRect = new Rect(Screen.width / 2 - width / 2, (int) Screen.height - height * 1.5f, width, height);

    }

    void OnGUI() {
        GUI.DrawTexture(spellBarRect, spellBar);



        CheckGUIFocused();
    }

    void CheckGUIFocused() {
        Vector3 mp = Input.mousePosition;
        Vector2 mousePos = new Vector2(mp.x, Screen.height-mp.y);
        if (GUIUtility.hotControl != 0 ||
            spellBarRect.Contains(mousePos) ||
            false) {
            GuiUtils.SetGUIFocused(true);
        }
    }

}
