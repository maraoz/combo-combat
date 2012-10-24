package ar.com.game.handler.session;

import ar.com.game.backend.ClientBackend;
import ar.com.game.handler.base.BaseClientHandler;
import ar.com.game.network.dispatch.MessageListener;
import ar.com.game.network.message.session.DisconnectNotify;

public class ClientDisconnectNotifyHandler extends BaseClientHandler implements
		MessageListener<DisconnectNotify> {

	private static final ClientDisconnectNotifyHandler INSTANCE = new ClientDisconnectNotifyHandler();

	private ClientDisconnectNotifyHandler() {
	}

	public static ClientDisconnectNotifyHandler get() {
		return INSTANCE;
	}

	public void handle(DisconnectNotify message) {
		ClientBackend.removePlayer(message.getWho());
		System.out.println(message.getWho() + " disconnected.");
	}

}
