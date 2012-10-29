package ar.com.nuchon.handler.session;

import ar.com.nuchon.handler.base.BaseClientHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.session.DisconnectResponse;

public class ClientDisconnectResponseHandler extends BaseClientHandler implements
		MessageListener<DisconnectResponse> {

	private static final ClientDisconnectResponseHandler INSTANCE = new ClientDisconnectResponseHandler();

	private ClientDisconnectResponseHandler() {
	}

	public static ClientDisconnectResponseHandler get() {
		return INSTANCE;
	}

	public void handle(DisconnectResponse message) {
		System.out.println("Disconnected from server gracefully :)");
	}

}
