using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HudController : MonoBehaviour {

    public Texture2D spellBar;
    public Texture2D lifeBarFrame;
    public Texture2D lifeBarFront;
    public Texture2D lifeBarBack;

    public float padding = 30;

    public float spellHpad;
    public float spellVPad;
    public float spellMargin;
    public float hotkeyVPad;
    public Texture2D cooldownOverlay;
    public Texture2D cooldownBack;
    public Texture2D empty;

    // tooltip
    public float tooltipWidth = 100;
    public float tooltipHeight = 180;
    private string currentTooltip;
    private bool showTooltip = false;
    private Rect tooltipRect;

    // portrait
    public Texture2D portrait;
    private Rect portraitRect;
    public int portraitHeight;
    public int portraitWidth;

    public int spellsShown = 4;

    private List<SpellCaster> spells;
    private MageLifeController life;
    private UserInputController controls;

    private GUIStyle hotkeyStyle;
    private GUIStyle spellIconStyle;
    private Rect spellBarRect;
    private Rect lifeBarRect;
    private Rect spellRect;

    public float quitWidth;
    public float quitHeight;
    private Rect quitRect;

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

        int spellBarWidth = spellBar.width /4;
        int spellBarHeight = spellBar.height /4;
        float lifeBarWidth = lifeBarBack.width*0.237f;
        float lifeBarHeight = lifeBarBack.height / 4;
        spellBarRect = new Rect(Screen.width / 2 - spellBarWidth / 2, padding+(int) Screen.height - spellBarHeight * 1.0f, spellBarWidth, spellBarHeight);
        lifeBarRect = new Rect(Screen.width / 2 - lifeBarWidth / 2, padding + (int) Screen.height - spellBarHeight * 0.95f, lifeBarWidth, lifeBarHeight);

        // spell bar
        GUI.DrawTexture(spellBarRect, spellBar);


        // spell
        float fSize = spellBarRect.height * 0.3f;
        spellRect = new Rect(spellBarRect.x + spellHpad - spellMargin, spellBarRect.y + spellVPad, fSize, fSize);
        for (int i = 0; i < spells.Count; i++) {
            SpellCaster spell = spells[i];

            // button
            float frameSize = fSize * 1.4f;
            Rect frameRect = new Rect(spellRect.x + spellMargin-5, spellRect.y-5, frameSize, frameSize);
            spellRect = new Rect(spellRect.x + spellMargin, spellRect.y, fSize, fSize);
            GUI.DrawTexture(frameRect, spell.GetFrame());
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
            string hotkey = spell.GetHotkeyString();
            Rect hotkeyRect = new Rect(spellRect);
            hotkeyRect.y += hotkeyVPad;
            //GUI.Label(hotkeyRect, hotkey, hotkeyStyle);

        }

        // non assigned spells
        for (int i = 0; i < spellsShown - spells.Count; i++) {
            spellRect = new Rect(spellRect.x + spellMargin, spellRect.y, fSize, fSize);
            GUI.DrawTexture(spellRect, empty);
        }



        // life bar
        Rect lifeBarFrontRect = new Rect(lifeBarRect);
        lifeBarFrontRect.width *= life.GetLifePercentage();
        GUI.DrawTexture(lifeBarFrontRect, lifeBarFront);
        GUI.DrawTexture(lifeBarRect, lifeBarBack);
        //GUI.DrawTexture(lifeBarRect, lifeBarFrame);

        // tooltip
        if (Event.current.type == EventType.Repaint) {
            currentTooltip = GUI.tooltip;
            if (currentTooltip == "") {
                showTooltip = false;
            }
        }
        if (showTooltip) {
            Vector3 mousePos = Input.mousePosition;
            tooltipRect = new Rect(mousePos.x, Screen.height - mousePos.y - tooltipHeight, tooltipWidth, tooltipHeight);
            GUILayout.Window(GameConstants.WIN_ID_TOOLTIP, tooltipRect, MakeTooltipWindow, "", GUI.skin.GetStyle("Box"));
        }
        if (currentTooltip != "" && !showTooltip) {
            showTooltip = true;
        }

        // quit button
        quitRect = new Rect(Screen.width - quitWidth, Screen.height - quitHeight, quitWidth, quitHeight);
        if (GUI.Button(quitRect, "Leave Game")) {
            // dissconnect from server
            Network.CloseConnection(Network.connections[0], true);
        }

        // portrait
        portraitRect = new Rect(0, Screen.height - portraitHeight, portraitWidth, portraitHeight);
        GUI.DrawTexture(portraitRect, portrait);


        CheckGUIFocused();
    }

    void MakeTooltipWindow(int id) {
        foreach (SpellCaster spell in spells) {
            if (currentTooltip == spell.GetTooltip().id) {
                InnerMakeTooltipWindow(spell);
                break;
            }
        }
    }

    void InnerMakeTooltipWindow(SpellCaster spell) {
        Tooltip tooltip = spell.GetTooltip();
        tooltip.SetSpell(spell);
        tooltip.Render();

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
