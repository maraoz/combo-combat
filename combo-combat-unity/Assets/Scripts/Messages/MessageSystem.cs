using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageSystem : MonoBehaviour {

    public int maxEntriesShown = 6;
    public int chatWindowHeight = 400;

    private Rect chatRect;
    public int chatWidth = 150;
    public int chatHPad = 10;
    public int chatVPad = 10;
    private Rect textInputRect;
    private bool showChat = true;
    public float chatFadeTime = 10;
    private float chatFadeTimeSpent = 0;
    private bool openingWindow;

    public Vector2 scrollPosition = Vector2.zero;
    private static string CHAT_INPUT_NAME = "Chat input field";
    private static string SCROLLVIEW_NAME = "Scrollview";
    private string inputField = "";
    private List<ChatEntry> entries = new List<ChatEntry>();
    private string chatUsername;

    private string lastFocusedGUI;

    class ChatEntry {
        public string sender = "";
        public string text = "";
        public Color color;
    }

    void Awake() {
        chatUsername = UsernameHolder.MyUsername();
    }

    void Update() {
        if (showChat) {
            if (lastFocusedGUI != CHAT_INPUT_NAME && inputField == "" && !Input.GetMouseButton(MouseButton.LEFT)) {
                chatFadeTimeSpent += Time.deltaTime;
                if (chatFadeTimeSpent >= chatFadeTime) {
                    chatFadeTimeSpent = 0;
                    showChat = false;
                }
            } else {
                chatFadeTimeSpent = 0;
            }
        }
    }

    void OnGUI() {
        lastFocusedGUI = GUI.GetNameOfFocusedControl();

        // check chat input logic
        if (Event.current.type == EventType.keyDown && Event.current.character == '\n') {
            scrollPosition = new Vector2(Mathf.Infinity, Mathf.Infinity);
            if (!showChat) {
                showChat = true;
            }

            if (GUI.GetNameOfFocusedControl() == CHAT_INPUT_NAME) {
                if (inputField.Length > 0) {
                    AddMessage(chatUsername, inputField, Color.white, GameConstants.NO_PLAYER, true);
                    inputField = "";
                }
                GUI.FocusControl("");
            } else {
                openingWindow = true;
            }
        }

        if (showChat) {
            chatRect = new Rect(chatHPad, Screen.height - (chatVPad + chatWindowHeight), chatWidth, chatWindowHeight);
            GUILayout.Window(GameConstants.CHAT_WIN_ID, chatRect, MakeChatWindow, "", new GUIStyle());
        }

        // GUI Focused update
        CheckGUIFocused();
    }


    void MakeChatWindow(int id) {

        GUILayout.BeginVertical();

        // render messages
        GUI.SetNextControlName(SCROLLVIEW_NAME);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.FlexibleSpace();
        for (int i = 0; i < entries.Count; i++) {
            ChatEntry entry = entries[i];
            string message = "[" + entry.sender + "]: " + entry.text;
            GUIStyle entryStyle = new GUIStyle();
            entryStyle.normal.textColor = entry.color;
            entryStyle.wordWrap = true;
            GUILayout.Label(message, entryStyle);
        }
        GUILayout.EndScrollView();

        // render chat input
        GUI.SetNextControlName(CHAT_INPUT_NAME);
        inputField = GUILayout.TextField(inputField, GUILayout.Width(chatRect.width*0.8f));

        if (openingWindow) {
            GUI.FocusControl(CHAT_INPUT_NAME);
            openingWindow = false;
        }

        GUILayout.EndVertical();

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
        AddSystemMessage(text, GameConstants.NO_PLAYER, false);
    }

    public void AddSystemMessageBroadcast(string text) {
        AddSystemMessage(text, GameConstants.NO_PLAYER, true);
    }

    // INTERNAL 
    private void AddSystemMessage(string text, NetworkPlayer player, bool broadcast) {
        AddMessage("System", text, Color.cyan, player, broadcast);
    }

    private void AddMessage(string sender, string text, Color color, NetworkPlayer player, bool broadcast) {
        Vector3 colorVec = new Vector3(color.r, color.g, color.b);
        if (player.Equals(GameConstants.NO_PLAYER)) {
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

        showChat = true;
        scrollPosition = new Vector2(Mathf.Infinity, Mathf.Infinity);
        chatFadeTimeSpent = 0;

        var entry = new ChatEntry();
        entry.sender = sender;
        entry.text = text;
        entry.color = new Color(color.x, color.y, color.z);
        entries.Add(entry);
        audio.Play();

        if (entries.Count > maxEntriesShown)
            entries.RemoveAt(0);

    }
}
