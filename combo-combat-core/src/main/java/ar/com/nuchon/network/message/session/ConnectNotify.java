package ar.com.nuchon.network.message.session;

import ar.com.nuchon.network.BaseMessage;

public class ConnectNotify extends BaseMessage {
	
	private final String who;

	public ConnectNotify(String who) {
		super();
		this.who = who;
	}
	
	public String getWho() {
		return who;
	}
	

}
