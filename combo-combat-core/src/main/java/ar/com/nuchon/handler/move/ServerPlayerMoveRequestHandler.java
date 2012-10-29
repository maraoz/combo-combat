package ar.com.nuchon.handler.move;

import ar.com.nuchon.backend.ServerBackend;
import ar.com.nuchon.handler.base.BaseServerHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.move.PlayerMoveNotify;
import ar.com.nuchon.network.message.move.PlayerMoveRequest;

public class ServerPlayerMoveRequestHandler extends BaseServerHandler implements
		MessageListener<PlayerMoveRequest> {

	private static final ServerPlayerMoveRequestHandler INSTANCE = new ServerPlayerMoveRequestHandler();

	public static ServerPlayerMoveRequestHandler get() {
		return INSTANCE;
	}

	private ServerPlayerMoveRequestHandler() {
	}

	public void handle(PlayerMoveRequest message) {
		Long sessionId = getClient(message.getChannel());
		ServerBackend.movePlayer(sessionId, message.getDelta());
		sendToAll(new PlayerMoveNotify(sessionId, ServerBackend.getPlayer(sessionId).getPosition()));
	}

}
