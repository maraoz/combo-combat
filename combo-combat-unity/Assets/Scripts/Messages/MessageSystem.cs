using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageSystem : MonoBehaviour {
    public int maxEntriesShown = 5;

    public int entryHeight = 25;
    public int entryWidth = 150;
    public int entryHPad = 10;
    public int entryVPad = 50;

    private bool chatInputFocused = false;
    private string inputField = "";
    private List<ChatEntry> entries = new List<ChatEntry>();

    class ChatEntry {
        public string sender = "";
        public string text = "";
        public bool mine = true;
    }


    void OnGUI() {
        if (Event.current.type == EventType.keyDown && Event.current.character == '\n') {
            if (chatInputFocused) {
                if (inputField.Length > 0) {
                    ApplyGlobalChatText(inputField, 1);
                    networkView.RPC("ApplyGlobalChatText", RPCMode.Others, inputField, 0);
                    inputField = "";
                }
                chatInputFocused = false;
            } else {
                chatInputFocused = true;
                GUI.FocusControl("Chat input field");
            }
        }

        for (int i = 0; i < entries.Count; i++) {
            ChatEntry entry = entries[entries.Count-1- i];
            GUI.Label(new Rect(entryHPad, Screen.height - (entryVPad + entryHeight * (i + 1)), entryWidth, entryHeight), entry.text);
        }
        if (chatInputFocused) {
            GUI.SetNextControlName("Chat input field");
            inputField = GUI.TextField(new Rect(entryHPad, Screen.height - entryVPad, entryWidth, entryHeight), inputField);
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
