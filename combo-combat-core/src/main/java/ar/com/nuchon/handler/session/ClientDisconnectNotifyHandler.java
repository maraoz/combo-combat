package ar.com.nuchon.handler.session;

import ar.com.nuchon.backend.ClientBackend;
import ar.com.nuchon.handler.base.BaseClientHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.session.DisconnectNotify;

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
