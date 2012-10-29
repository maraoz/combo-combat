package ar.com.nuchon.handler.move;

import ar.com.nuchon.backend.ClientBackend;
import ar.com.nuchon.backend.domain.Vector2D;
import ar.com.nuchon.handler.base.BaseClientHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.move.PlayerMoveNotify;

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
