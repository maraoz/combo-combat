using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageSystem : MonoBehaviour {
    public int maxEntriesShown = 5;

    public int entryHeight = 25;
    public int entryWidth = 150;
    public int entryHPad = 10;
    public int entryVPad = 50;
    private Rect textInputRect;

    private static string CHAT_INPUT_NAME = "Chat input field";
    private bool showChatInput = true;
    private string inputField = "";
    private List<ChatEntry> entries = new List<ChatEntry>();

    class ChatEntry {
        public string sender = "";
        public string text = "";
        public bool mine = true;
    }

    void Awake() {
        textInputRect = new Rect(entryHPad, Screen.height - entryVPad, entryWidth, entryHeight);
    }


    void OnGUI() {
        if (Event.current.type == EventType.keyDown && Event.current.character == '\n') {
            if (showChatInput) {
                if (inputField.Length > 0) {
                    ApplyGlobalChatText(inputField, 1);
                    networkView.RPC("ApplyGlobalChatText", RPCMode.Others, inputField, 0);
                    inputField = "";
                }
                showChatInput = false;
                GUI.FocusControl("");
            } else {
                showChatInput = true;
                GUI.FocusControl(CHAT_INPUT_NAME);
            }
        }

        for (int i = 0; i < entries.Count; i++) {
            ChatEntry entry = entries[entries.Count - 1 - i];
            GUI.Label(new Rect(entryHPad, Screen.height - (entryVPad + entryHeight * (i + 1)), entryWidth, entryHeight), entry.text);
        }
        if (showChatInput) {
            GUI.SetNextControlName(CHAT_INPUT_NAME);
            inputField = GUI.TextField(textInputRect, inputField);
        }

        CheckGUIFocused();
    }

    void CheckGUIFocused() {

        if (GUIUtility.hotControl != 0) {
            GuiUtils.SetGUIFocused(true);
        }
    }


    [RPC]
    void ApplyGlobalChatText(string str, int mine) {
        var entry = new ChatEntry();
        entry.sender = "Not implemented";
        entry.text = str;
        if (mine == 1) entry.mine = true;
        else entry.mine = false;

        entries.Add(entry);

        if (entries.Count > maxEntriesShown)
            entries.RemoveAt(0);

    }
}
