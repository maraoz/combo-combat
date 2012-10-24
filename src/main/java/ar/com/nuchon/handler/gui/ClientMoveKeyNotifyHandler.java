package ar.com.nuchon.handler.gui;

import ar.com.nuchon.handler.base.BaseClientHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.gui.MoveKeyNotify;
import ar.com.nuchon.network.message.move.PlayerMoveRequest;

public class ClientMoveKeyNotifyHandler extends BaseClientHandler implements
		MessageListener<MoveKeyNotify> {

	private static final ClientMoveKeyNotifyHandler INSTANCE = new ClientMoveKeyNotifyHandler();

	private ClientMoveKeyNotifyHandler() {
	}

	public static ClientMoveKeyNotifyHandler get() {
		return INSTANCE;
	}

	public void handle(MoveKeyNotify message) {
		send(new PlayerMoveRequest(message.getPosition()));
	}

}
