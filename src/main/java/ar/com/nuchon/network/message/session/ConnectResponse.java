package ar.com.nuchon.network.message.session;

import ar.com.nuchon.network.BaseMessage;

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
