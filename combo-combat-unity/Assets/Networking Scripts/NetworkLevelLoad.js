function Awake ()
{
	// Network level loading is done in a seperate channel.
	//DontDestroyOnLoad(this);
}

function OnConnectedToServer() {
    Network.SetSendingEnabled(0, false);	
    Network.isMessageQueueRunning = false;
    Application.LoadLevel("ComboGame");
}

function OnServerInitialized() {
    Application.LoadLevel("ComboGame");
    Debug.Log("Server initialized and ready");
}
function OnDisconnectedFromServer ()
{
    //Application.LoadLevel(disconnectedLevel);
    // TODO: Reconnecting...
}

@script RequireComponent(NetworkView)
