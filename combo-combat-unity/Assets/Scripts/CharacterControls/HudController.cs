using UnityEngine;
using System.Collections;

public class HudController : MonoBehaviour {

    public Texture2D spellBar;
    public Texture2D lifeBarFrame;
    public Texture2D lifeBarFront;
    public Texture2D lifeBarBack;

    private MageLifeController life;

    private Rect spellBarRect;
    private Rect lifeBarRect;

    void Awake() {
        int spellBarWidth = spellBar.width / 4;
        int spellBarHeight = spellBar.height / 4;
        int lifeBarWidth = lifeBarFrame.width;
        int lifeBarHeight = (int)(lifeBarFrame.height*0.8f);
        spellBarRect = new Rect(Screen.width / 2 - spellBarWidth / 2, (int) Screen.height - spellBarHeight * 1.0f, spellBarWidth, spellBarHeight);
        lifeBarRect = new Rect(Screen.width / 2 - lifeBarWidth / 2, (int) Screen.height - spellBarHeight * 1.74f, lifeBarWidth, lifeBarHeight);
    }

    void OnGUI() {
        GUI.DrawTexture(spellBarRect, spellBar);

        Rect lifeBarFrontRect = new Rect(lifeBarRect);
        lifeBarFrontRect.width *= life.GetLifePercentage();
        GUI.DrawTexture(lifeBarFrontRect, lifeBarFront);

        GUI.DrawTexture(lifeBarRect, lifeBarFrame);

        CheckGUIFocused();
    }

    void CheckGUIFocused() {
        Vector3 mp = Input.mousePosition;
        Vector2 mousePos = new Vector2(mp.x, Screen.height - mp.y);
        if (GUIUtility.hotControl != 0 ||
            spellBarRect.Contains(mousePos) ||
            lifeBarRect.Contains(mousePos)) {
            GuiUtils.SetGUIFocused(true);
        }
    }

    public void SetLifeController(MageLifeController lifeController) {
        life = lifeController;
    }

}
