using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageSystem : MonoBehaviour {

    public int maxEntriesShown = 6;

    public int entryHeight = 25;
    public int entryWidth = 150;
    public int entryHPad = 10;
    public int entryVPad = 10;
    private Rect textInputRect;

    private static string CHAT_INPUT_NAME = "Chat input field";
    private bool showChatInput = true;
    private string inputField = "";
    private List<ChatEntry> entries = new List<ChatEntry>();
    private string chatUsername;

    private NetworkPlayer NO_PLAYER = new NetworkPlayer();

    class ChatEntry {
        public string sender = "";
        public string text = "";
        public Color color;
    }

    void Awake() {
        chatUsername = UsernameHolder.MyUsername();
    }


    void OnGUI() {
        textInputRect = new Rect(entryHPad, Screen.height - entryVPad, entryWidth, entryHeight);

        // check chat input logic
        if (Event.current.type == EventType.keyDown && Event.current.character == '\n') {
            if (showChatInput) {
                if (inputField.Length > 0) {
                    AddMessage(chatUsername, inputField, Color.white, NO_PLAYER, true);
                    inputField = "";
                }
                showChatInput = false;
                GUI.FocusControl("");
            } else {
                showChatInput = true;
                GUI.FocusControl(CHAT_INPUT_NAME);
            }
        }

        // render messages
        for (int i = 0; i < entries.Count; i++) {
            ChatEntry entry = entries[entries.Count - 1 - i];
            Rect entryRect = new Rect(entryHPad, Screen.height - (entryVPad + entryHeight * (i + 1)), entryWidth, entryHeight);
            string message = "[" + entry.sender + "]: " + entry.text;
            GUIStyle entryStyle = new GUIStyle();
            entryStyle.normal.textColor = entry.color;
            GUI.Label(entryRect, message, entryStyle);
        }

        // render chat
        if (showChatInput) {
            GUI.SetNextControlName(CHAT_INPUT_NAME);
            inputField = GUI.TextField(textInputRect, inputField);
        }

        // GUI Focused update
        CheckGUIFocused();
    }

    void CheckGUIFocused() {
        if (GUIUtility.hotControl != 0) {
            GuiUtils.SetGUIFocused(true);
        }
    }

    // ACCESSIBLE API
    public void AddSystemMessageTo(NetworkPlayer player, string text) {
        AddSystemMessage(text, player, false);
    }

    public void AddSystemMessageSelf(string text) {
        AddSystemMessage(text, NO_PLAYER, false);
    }

    public void AddSystemMessageBroadcast(string text) {
        AddSystemMessage(text, NO_PLAYER, true);
    }

    // INTERNAL 
    private void AddSystemMessage(string text, NetworkPlayer player, bool broadcast) {
        AddMessage("System", text, Color.cyan, player, broadcast);
    }

    private void AddMessage(string sender, string text, Color color, NetworkPlayer player, bool broadcast) {
        Vector3 colorVec = new Vector3(color.r, color.g, color.b);
        if (player.Equals(NO_PLAYER)) {
            if (broadcast) {
                networkView.RPC("DoAddMessage", RPCMode.All, sender, text, colorVec);
            } else {
                DoAddMessage(sender, text, colorVec);
            }
        } else {
            networkView.RPC("DoAddMessage", player, sender, text, colorVec);
        }
    }

    [RPC]
    void DoAddMessage(string sender, string text, Vector3 color) {
        var entry = new ChatEntry();
        entry.sender = sender;
        entry.text = text;
        entry.color = new Color(color.x, color.y, color.z);
        entries.Add(entry);

        if (entries.Count > maxEntriesShown)
            entries.RemoveAt(0);

    }
}
