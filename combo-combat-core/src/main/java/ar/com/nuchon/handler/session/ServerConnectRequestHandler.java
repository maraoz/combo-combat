package ar.com.nuchon.handler.session;

import ar.com.nuchon.backend.ServerBackend;
import ar.com.nuchon.handler.base.BaseServerHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.session.ConnectNotify;
import ar.com.nuchon.network.message.session.ConnectRequest;
import ar.com.nuchon.network.message.session.ConnectResponse;

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
