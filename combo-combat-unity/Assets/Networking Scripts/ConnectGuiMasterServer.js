
var gameName = "You must change this";
var serverPort = 25002;
var allowsDedicatedServer = false;

private var timeoutHostList = 0.0;
private var lastHostListRequest = -1000.0;
private var hostListRefreshTimeout = 10.0;

private var connectionTestResult : ConnectionTesterStatus = ConnectionTesterStatus.Undetermined;
private var filterNATHosts = false;
private var probingPublicIP = false;
private var doneTesting = false;
private var timer : float = 0.0;
private var useNat = false;		// Should the server enabled NAT punchthrough feature

private var windowRect;
private var serverListRect;
private var testMessage = "Undetermined NAT capabilities";

// Enable this if not running a client on the server machine
//MasterServer.dedicatedServer = true; 

function OnFailedToConnectToMasterServer(info: NetworkConnectionError)
{
	Debug.Log(info);
}

function OnFailedToConnect(info: NetworkConnectionError)
{
	Debug.Log(info);
}

function OnGUI ()
{
    if (Network.peerType == NetworkPeerType.Disconnected || Network.isServer) {
        windowRect = GUILayout.Window (0, windowRect, MakeWindow, "Server Controls");
    }
	if (Network.peerType == NetworkPeerType.Disconnected)
		serverListRect = GUILayout.Window(1, serverListRect, MakeClientWindow, "Server List");
}

function Awake ()
{
	windowRect = Rect(Screen.width-300,0,300,100);
	serverListRect = Rect(0, 0, Screen.width - windowRect.width, 100);
	// Start connection test
	connectionTestResult = Network.TestConnection();
	
	// What kind of IP does this machine have? TestConnection also indicates this in the
	// test results
	if (Network.HavePublicAddress())
		Debug.Log("This machine has a public IP address");
	else
		Debug.Log("This machine has a private IP address");
}

function Update()
{
	// If test is undetermined, keep running
	if (!doneTesting)
	    TestConnection();
	if (Time.realtimeSinceStartup > lastHostListRequest + hostListRefreshTimeout) {
	    RefreshServers();
	}
}

function TestConnection()
{
	// Start/Poll the connection test, report the results in a label and react to the results accordingly
	connectionTestResult = Network.TestConnection();
	switch (connectionTestResult)
	{
		case ConnectionTesterStatus.Error: 
			testMessage = "Problem determining NAT capabilities";
			doneTesting = true;
			break;
			
		case ConnectionTesterStatus.Undetermined: 
			testMessage = "Undetermined NAT capabilities";
			doneTesting = false;
			break;
						
		case ConnectionTesterStatus.PublicIPIsConnectable:
			testMessage = "Directly connectable public IP address.";
			useNat = false;
			doneTesting = true;
			break;
			
		// This case is a bit special as we now need to check if we can 
		// circumvent the blocking by using NAT punchthrough
		case ConnectionTesterStatus.PublicIPPortBlocked:
			testMessage = "Non-connectble public IP address (port " + serverPort +" blocked), running a server is impossible.";
			useNat = false;
			// If no NAT punchthrough test has been performed on this public IP, force a test
			if (!probingPublicIP)
			{
				Debug.Log("Testing if firewall can be circumvented");
				connectionTestResult = Network.TestConnectionNAT();
				probingPublicIP = true;
				timer = Time.time + 10;
			}
			// NAT punchthrough test was performed but we still get blocked
			else if (Time.time > timer)
			{
				probingPublicIP = false; 		// reset
				useNat = true;
				doneTesting = true;
			}
			break;
		case ConnectionTesterStatus.PublicIPNoServerStarted:
			testMessage = "Public IP address but server not initialized, it must be started to check server accessibility. Restart connection test when ready.";
			break;
			
		case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
			Debug.Log("LimitedNATPunchthroughPortRestricted");
			testMessage = "Limited NAT punchthrough capabilities. Cannot connect to all types of NAT servers.";
			useNat = true;
			doneTesting = true;
			break;
					
		case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
			Debug.Log("LimitedNATPunchthroughSymmetric");
			testMessage = "Limited NAT punchthrough capabilities. Cannot connect to all types of NAT servers. Running a server is ill adviced as not everyone can connect.";
			useNat = true;
			doneTesting = true;
			break;
		
		case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
		case ConnectionTesterStatus.NATpunchthroughFullCone:
			Debug.Log("NATpunchthroughAddressRestrictedCone || NATpunchthroughFullCone");
			testMessage = "NAT punchthrough capable. Can connect to all servers and receive connections from all clients. Enabling NAT punchthrough functionality.";
			useNat = true;
			doneTesting = true;
			break;

		default: 
			testMessage = "Error in test routine, got " + connectionTestResult;
	}
}

function RefreshServers() {
    MasterServer.RequestHostList (gameName);
    lastHostListRequest = Time.realtimeSinceStartup;
}

function MakeWindow (id : int)
{	
	if (Network.peerType == NetworkPeerType.Disconnected)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		// Start a new server
		if (allowsDedicatedServer)
		{
		    if (IsBatchMode() || GUILayout.Button ("Start Server")) {
			    Network.InitializeServer(32, serverPort, useNat);
			    MasterServer.RegisterHost(gameName, "Main Server", "Argentina public server");
		    }
		}

		// Refresh hosts
		if (GUILayout.Button ("Refresh available Servers"))
		{
		    RefreshServers();
		}
		
		GUILayout.FlexibleSpace();
		
		GUILayout.EndHorizontal();
	}
	else
	{
		if (GUILayout.Button ("Disconnect"))
		{
			Network.Disconnect();
			MasterServer.UnregisterHost();
		}
		GUILayout.FlexibleSpace();
	}
}

function IsBatchMode(){
    var cla = System.Environment.GetCommandLineArgs();
    return cla.Length > 1 && cla[1] == "-batchmode";
}

function MakeClientWindow(id : int)
{
	GUILayout.Space(5);

	var data : HostData[] = MasterServer.PollHostList();
	var count = 0;
	for (var element in data)
	{
		GUILayout.BeginHorizontal();

		// Do not display NAT enabled games if we cannot do NAT punchthrough
		if ( !(filterNATHosts && element.useNat) )
		{
			var connections = element.connectedPlayers + "/" + element.playerLimit;
			GUILayout.Label(element.gameName);
			GUILayout.Space(5);
			GUILayout.Label(connections);
			GUILayout.Space(5);
			var hostInfo = "";
			
			// Indicate if NAT punchthrough will be performed, omit showing GUID
			if (element.useNat)
			{
				GUILayout.Label("NAT");
				GUILayout.Space(5);
			}
			// Here we display all IP addresses, there can be multiple in cases where
			// internal LAN connections are being attempted. In the GUI we could just display
			// the first one in order not confuse the end user, but internally Unity will
			// do a connection check on all IP addresses in the element.ip list, and connect to the
			// first valid one.
			for (var host in element.ip)
				hostInfo = hostInfo + host + ":" + element.port + " ";
			
			//GUILayout.Label("[" + element.ip + ":" + element.port + "]");	
			GUILayout.Label(hostInfo);	
			GUILayout.Space(5);
			GUILayout.Label(element.comment);
			GUILayout.Space(5);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Connect")) {
			    Network.Connect(element);
			    this.enabled = false;
			}
		}
		GUILayout.EndHorizontal();	
	}
}
