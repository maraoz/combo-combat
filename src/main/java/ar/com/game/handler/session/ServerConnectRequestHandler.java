package ar.com.game.handler.session;

import ar.com.game.backend.ServerBackend;
import ar.com.game.handler.base.BaseServerHandler;
import ar.com.game.network.dispatch.MessageListener;
import ar.com.game.network.message.session.ConnectNotify;
import ar.com.game.network.message.session.ConnectRequest;
import ar.com.game.network.message.session.ConnectResponse;

public class ServerConnectRequestHandler extends BaseServerHandler implements
		MessageListener<ConnectRequest> {

	private static final ServerConnectRequestHandler INSTANCE = new ServerConnectRequestHandler();

	public static ServerConnectRequestHandler get() {
		return INSTANCE;
	}

	private ServerConnectRequestHandler() {
	}

	public void handle(ConnectRequest message) {
		Long sessionId = addClient(message.getChannel());
		ServerBackend.addPlayer(sessionId);
		send(new ConnectResponse(sessionId), sessionId);
		sendToAll(new ConnectNotify(sessionId.toString()));
	}

}
