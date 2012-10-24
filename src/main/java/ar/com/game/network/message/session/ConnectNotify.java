package ar.com.game.network.message.session;

import ar.com.game.network.BaseMessage;

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
