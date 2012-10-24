package ar.com.game.handler.move;

import ar.com.game.backend.ClientBackend;
import ar.com.game.backend.domain.Vector2D;
import ar.com.game.handler.base.BaseClientHandler;
import ar.com.game.network.dispatch.MessageListener;
import ar.com.game.network.message.move.PlayerMoveNotify;

public class ClientPlayerMoveNotifyHandler extends BaseClientHandler implements
		MessageListener<PlayerMoveNotify> {

	private static final ClientPlayerMoveNotifyHandler INSTANCE = new ClientPlayerMoveNotifyHandler();

	private ClientPlayerMoveNotifyHandler() {
	}

	public static ClientPlayerMoveNotifyHandler get() {
		return INSTANCE;
	}

	public void handle(PlayerMoveNotify message) {
		Long who = message.getWho();
		Vector2D where = message.getWhere();
		
		ClientBackend.movePlayer(who, where);
	}

}
