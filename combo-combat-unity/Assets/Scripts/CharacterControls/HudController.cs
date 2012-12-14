using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HudController : MonoBehaviour {

    public Texture2D spellBar;
    public Texture2D lifeBarFrame;
    public Texture2D lifeBarFront;
    public Texture2D lifeBarBack;

    public float spellHpad;
    public float spellVPad;
    public float spellMargin;
    public Texture2D empty;

    public int spellsShown = 4;

    private List<SpellCaster> spells;
    private MageLifeController life;
    private ClickPlayerMovementScript controls;

    private Rect spellBarRect;
    private Rect lifeBarRect;
    private Rect spellRect;

    void Awake() {
        int spellBarWidth = spellBar.width / 4;
        int spellBarHeight = spellBar.height / 4;
        int lifeBarWidth = lifeBarFrame.width;
        int lifeBarHeight = (int) (lifeBarFrame.height * 0.8f);
        spellBarRect = new Rect(Screen.width / 2 - spellBarWidth / 2, (int) Screen.height - spellBarHeight * 1.0f, spellBarWidth, spellBarHeight);
        lifeBarRect = new Rect(Screen.width / 2 - lifeBarWidth / 2, (int) Screen.height - spellBarHeight * 1.74f, lifeBarWidth, lifeBarHeight);
    }

    void OnGUI() {
        float fSize = spellBarRect.height * 0.73f;
        GUIStyle spellButtonStyle = GUI.skin.button;
        spellButtonStyle.padding = new RectOffset(0, 0, 0, 0);
        spellRect = new Rect(spellBarRect.x + spellHpad - spellMargin, spellBarRect.y + spellVPad, fSize, fSize);
        for (int i = 0; i < spells.Count; i++) {
            SpellCaster spell = spells[i];
            spellRect = new Rect(spellRect.x + spellMargin, spellRect.y, fSize, fSize);
            if (GUI.Button(spellRect, new GUIContent(spell.GetIcon(), spell.GetTooltip()), spellButtonStyle)) {
                controls.SimulateSpellHotkey(spell);
            }
        }
        for (int i = 0; i < spellsShown - spells.Count; i++) {
            spellRect = new Rect(spellRect.x + spellMargin, spellRect.y, fSize, fSize);
            GUI.DrawTexture(spellRect, empty);
        }

        GUI.DrawTexture(spellBarRect, spellBar);

        Rect lifeBarFrontRect = new Rect(lifeBarRect);
        lifeBarFrontRect.width *= life.GetLifePercentage();
        GUI.DrawTexture(lifeBarRect, lifeBarBack);
        GUI.DrawTexture(lifeBarFrontRect, lifeBarFront);
        GUI.DrawTexture(lifeBarRect, lifeBarFrame);

        //Debug.Log(GUI.tooltip);


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

    public void SetMageOwner(GameObject mage) {
        life = mage.GetComponent<MageLifeController>();
        spells = mage.GetComponent<Mage>().GetSpellCasters();
        controls = mage.GetComponent<ClickPlayerMovementScript>();
    }

}
