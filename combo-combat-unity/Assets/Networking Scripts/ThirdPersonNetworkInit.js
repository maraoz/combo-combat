function OnNetworkInstantiate (msg : NetworkMessageInfo) {
	// This is our own player
	if (networkView.isMine)
	{
		Camera.main.SendMessage("SetTarget", transform);
		GetComponent("NetworkInterpolatedTransform").enabled = false;
	}
	// This is just some remote controlled player
	else
	{
		name += "Remote"+msg.sender;
		GetComponent(ThirdPersonController).enabled = false;
		GetComponent(ThirdPersonSimpleAnimation).enabled = false;
		GetComponent("NetworkInterpolatedTransform").enabled = true;
	}
}
