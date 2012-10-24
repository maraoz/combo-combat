package ar.com.game.handler.gui;

import ar.com.game.handler.base.BaseClientHandler;
import ar.com.game.network.dispatch.MessageListener;
import ar.com.game.network.message.gui.MouseMovedNotify;
import ar.com.game.network.message.move.PlayerMoveRequest;

public class ClientMouseMovedNotifyHandler extends BaseClientHandler implements
		MessageListener<MouseMovedNotify> {

	private static final ClientMouseMovedNotifyHandler INSTANCE = new ClientMouseMovedNotifyHandler();

	private ClientMouseMovedNotifyHandler() {
	}

	public static ClientMouseMovedNotifyHandler get() {
		return INSTANCE;
	}

	public void handle(MouseMovedNotify message) {
		send(new PlayerMoveRequest(message.getPosition()));
	}

}
