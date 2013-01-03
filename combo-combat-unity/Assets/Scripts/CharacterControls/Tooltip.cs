using UnityEngine;
using System.Collections;

public class Tooltip : MonoBehaviour {

    public string id;
    public string spellName;
    private SpellCaster spell;

    [Multiline]
    public string description;

    public void Render() {

        GUIStyle baseTextStyle = new GUIStyle();
        baseTextStyle.font = GUI.skin.GetStyle("PlainText").font;

        GUIStyle nameTextStyle = new GUIStyle(baseTextStyle);
        nameTextStyle.fontSize = 18;
        nameTextStyle.normal.textColor = Color.blue;

        GUIStyle hotkeyTextStyle = new GUIStyle(baseTextStyle);
        hotkeyTextStyle.fontSize = 24;
        hotkeyTextStyle.normal.textColor = Color.red;

        GUIStyle descrTextStyle = new GUIStyle(baseTextStyle);
        descrTextStyle.fontSize = 16;
        descrTextStyle.normal.textColor = Color.black;
        descrTextStyle.wordWrap = true;
        
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label(spellName, nameTextStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label(spell.GetHotkeyString(), hotkeyTextStyle);
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        description = description.Replace("\n", "");
        GUILayout.Label(description, descrTextStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

    }



    internal void SetSpell(SpellCaster spell) {
        this.spell = spell;
    }
}
