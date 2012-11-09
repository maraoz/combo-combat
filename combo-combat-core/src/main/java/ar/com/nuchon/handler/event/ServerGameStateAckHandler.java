package ar.com.nuchon.handler.event;

import ar.com.nuchon.backend.ServerBackend;
import ar.com.nuchon.handler.base.BaseServerHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.event.GameStateAck;

public class ServerGameStateAckHandler extends BaseServerHandler implements
		MessageListener<GameStateAck> {

	private static final ServerGameStateAckHandler INSTANCE = new ServerGameStateAckHandler();

	private ServerGameStateAckHandler() {
	}

	public static ServerGameStateAckHandler get() {
		return INSTANCE;
	}

	public void handle(GameStateAck message) {
		Long session = getClient(message.getChannel());
		ServerBackend.getSessionToLastAckState().put(session, message.getSequence());
	}

}
