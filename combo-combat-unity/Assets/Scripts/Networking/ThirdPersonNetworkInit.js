function OnNetworkInstantiate (msg : NetworkMessageInfo) {
	// This is our own player
	if (networkView.isMine)
	{
	    Camera.main.SendMessage("SetTarget", transform);
	    //Camera.main.transform.parent = transform;
		GetComponent("NetworkInterpolatedTransform").enabled = false;
	}
	// This is just some remote controlled player
	else
	{
		name += "Remote";
		GetComponent("ClickPlayerMovementScript").enabled = false;
		GetComponent("CharacterSimpleAnimation").enabled = false;
		GetComponent("NetworkInterpolatedTransform").enabled = true;
	}
}
