package ar.com.game.network.message.session;

import ar.com.game.network.BaseMessage;

public class DisconnectNotify extends BaseMessage {
	
	private final Long who;

	public DisconnectNotify(Long who) {
		super();
		this.who = who;
	}
	
	public Long getWho() {
		return who;
	}
	

}
