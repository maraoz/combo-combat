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
    public float hotkeyVPad;
    public Texture2D cooldownOverlay;
    public Texture2D cooldownBack;
    public Texture2D empty;

    public int spellsShown = 4;

    private List<SpellCaster> spells;
    private MageLifeController life;
    private ClickPlayerMovementScript controls;

    private Rect spellBarRect;
    private Rect lifeBarRect;
    private Rect spellRect;

    void Awake() {
    }

    void OnGUI() {
        GUI.skin = GUISkinProvider.GetSkin();
        int spellBarWidth = spellBar.width / 3;
        int spellBarHeight = spellBar.height / 3;
        int lifeBarWidth = lifeBarFrame.width;
        int lifeBarHeight = (int) (lifeBarFrame.height * 0.8f);
        spellBarRect = new Rect(Screen.width / 2 - spellBarWidth / 2, (int) Screen.height - spellBarHeight * 1.0f, spellBarWidth, spellBarHeight);
        lifeBarRect = new Rect(Screen.width / 2 - lifeBarWidth / 2, (int) Screen.height - spellBarHeight * 1.74f, lifeBarWidth, lifeBarHeight);
        // spell
        float fSize = spellBarRect.height * 0.74f;
        spellRect = new Rect(spellBarRect.x + spellHpad - spellMargin, spellBarRect.y + spellVPad, fSize, fSize);
        for (int i = 0; i < spells.Count; i++) {
            SpellCaster spell = spells[i];

            // button
            spellRect = new Rect(spellRect.x + spellMargin, spellRect.y, fSize, fSize);
            if (GUI.Button(spellRect, new GUIContent(spell.GetIcon(), spell.GetTooltip()), "Spell Icon")) {
                controls.OnSpellHotkeyPressed(spell);
            }

            // cooldown effect
            float percentage = spell.GetCooldownPercentage();
            if (percentage > 0) {
                GUI.DrawTexture(spellRect, cooldownBack);
                Rect cdRect = new Rect(spellRect);
                cdRect.height *= percentage;
                GUI.DrawTexture(cdRect, cooldownOverlay);
            }

            // hotkey
            string hotkey = ("" + System.Convert.ToChar(spell.GetHotkey())).ToUpper();
            Rect hotkeyRect = new Rect(spellRect);
            hotkeyRect.y += hotkeyVPad;
            GUI.Label(hotkeyRect, hotkey);
        }

        // non assigned spells
        for (int i = 0; i < spellsShown - spells.Count; i++) {
            spellRect = new Rect(spellRect.x + spellMargin, spellRect.y, fSize, fSize);
            GUI.DrawTexture(spellRect, empty);
        }

        // spell bar
        GUI.DrawTexture(spellBarRect, spellBar);


        // life bar
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
