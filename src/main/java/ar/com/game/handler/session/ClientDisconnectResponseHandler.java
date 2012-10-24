package ar.com.game.handler.session;

import ar.com.game.handler.base.BaseClientHandler;
import ar.com.game.network.dispatch.MessageListener;
import ar.com.game.network.message.session.DisconnectResponse;

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
