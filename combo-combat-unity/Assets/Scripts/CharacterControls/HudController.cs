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

    public float tooltipWidth = 100;
    public float tooltipHeight = 180;
    private string currentTooltip;
    private bool showTooltip = false;

    public int spellsShown = 4;

    private List<SpellCaster> spells;
    private MageLifeController life;
    private UserInputController controls;

    private GUIStyle hotkeyStyle;
    private GUIStyle spellIconStyle;
    private Rect spellBarRect;
    private Rect lifeBarRect;
    private Rect spellRect;

    void Awake() {
        enabled = false;
        currentTooltip = "";
    }

    void OnGUI() {
        GUI.skin = GUISkinProvider.GetSkin();

        // initialize styles once
        if (hotkeyStyle == null) {
            hotkeyStyle = GUI.skin.GetStyle("PlainText");
            spellIconStyle = GUI.skin.GetStyle("Spell Icon");
        }

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
            if (GUI.Button(spellRect, new GUIContent(spell.GetIcon(), spell.GetTooltip().id), spellIconStyle)) {
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
            GUI.Label(hotkeyRect, hotkey, hotkeyStyle);

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

        // tooltip
        if (Event.current.type == EventType.Repaint) {
            currentTooltip = GUI.tooltip;
            if (currentTooltip == "") {
                showTooltip = false;
            }
        }
        Vector3 mousePos = Input.mousePosition;
        Rect tooltipRect = new Rect(mousePos.x, Screen.height - mousePos.y - tooltipHeight, tooltipWidth, tooltipHeight);
        GUILayout.Window(GameConstants.TOOLTIP_WIN_ID, tooltipRect, MakeTooltipWindow, "", new GUIStyle());

        CheckGUIFocused();
    }

    void MakeTooltipWindow(int id) {
        if (currentTooltip != "" && !showTooltip) {
            showTooltip = true;
            return;
        }
        GUILayout.BeginHorizontal();
        foreach (SpellCaster spell in spells) {
            if (currentTooltip == spell.GetTooltip().id) {
                Tooltip tooltip = spell.GetTooltip();
                GUILayout.Label(tooltip.spellName);
                break;
            }
        }
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
        controls = mage.GetComponent<UserInputController>();
        enabled = true;
    }

}
