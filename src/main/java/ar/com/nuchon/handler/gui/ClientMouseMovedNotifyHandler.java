package ar.com.nuchon.handler.gui;

import ar.com.nuchon.handler.base.BaseClientHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.gui.MouseMovedNotify;
import ar.com.nuchon.network.message.move.PlayerMoveRequest;

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
