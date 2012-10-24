package ar.com.nuchon.handler.session;

import ar.com.nuchon.handler.base.BaseServerHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.session.DisconnectNotify;
import ar.com.nuchon.network.message.session.DisconnectRequest;
import ar.com.nuchon.network.message.session.DisconnectResponse;

public class ServerDisconnectRequestHandler extends BaseServerHandler implements
		MessageListener<DisconnectRequest> {

	private static final ServerDisconnectRequestHandler INSTANCE = new ServerDisconnectRequestHandler();

	public static ServerDisconnectRequestHandler get() {
		return INSTANCE;
	}

	private ServerDisconnectRequestHandler() {
	}

	public void handle(DisconnectRequest message) {
		Long sessionId = getClient(message.getChannel());
		sendAndTerminateConnection(new DisconnectResponse(), sessionId);
		removeClient(sessionId);
		sendToAll(new DisconnectNotify(sessionId));
	}

}
