using UnityEngine;
using System.Collections;

public class Tooltip : MonoBehaviour {

    public string id;
    public string spellName;
    private Spell spell;

    [Multiline]
    public string description;

    [Multiline]
    public string flavorText;

    public Color hotKeyColor = Color.white;
    public Color flavorColor = Color.white;

    public void Render() {

        GUIStyle baseTextStyle = new GUIStyle();
        baseTextStyle.font = GUI.skin.GetStyle("PlainText").font;

        GUIStyle nameTextStyle = new GUIStyle(baseTextStyle);
        nameTextStyle.fontSize = 18;
        nameTextStyle.normal.textColor = Color.blue;

        GUIStyle hotkeyTextStyle = new GUIStyle(baseTextStyle);
        hotkeyTextStyle.fontSize = 24;
        hotkeyTextStyle.normal.textColor = hotKeyColor;

        GUIStyle descrTextStyle = new GUIStyle(baseTextStyle);
        descrTextStyle.fontSize = 12;
        descrTextStyle.normal.textColor = Color.black;
        descrTextStyle.wordWrap = true;

        GUIStyle flavorTextStyle = new GUIStyle(baseTextStyle);
        flavorTextStyle.fontSize = 12;
        flavorTextStyle.normal.textColor = flavorColor;
        flavorTextStyle.wordWrap = true;
        
        GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(spellName, nameTextStyle);
                    GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.Label("["+spell.GetHotkeyString()+"]", hotkeyTextStyle);
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            description = description.Replace("\n", "");
            GUILayout.Label(description, descrTextStyle);
            GUILayout.FlexibleSpace();
            GUILayout.Label(flavorText, flavorTextStyle);
        GUILayout.EndVertical();

    }



    internal void SetSpell(Spell spell) {
        this.spell = spell;
    }
}
