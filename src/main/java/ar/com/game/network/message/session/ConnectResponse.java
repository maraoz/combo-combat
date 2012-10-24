package ar.com.game.network.message.session;

import ar.com.game.network.BaseMessage;

public class ConnectResponse extends BaseMessage {

	private final Long yourSession;

	public ConnectResponse(Long yourSession) {
		super();
		this.yourSession = yourSession;
	}

	public Long getYourSession() {
		return yourSession;
	}
	
}
