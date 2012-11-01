package ar.com.nuchon.handler.gui;

import ar.com.nuchon.handler.base.BaseClientHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.gui.ShootKeyNotify;
import ar.com.nuchon.network.message.spells.FireballCastRequest;

public class ClientShootKeyNotifyHandler extends BaseClientHandler implements
		MessageListener<ShootKeyNotify> {

	private static final ClientShootKeyNotifyHandler INSTANCE = new ClientShootKeyNotifyHandler();

	private ClientShootKeyNotifyHandler() {
	}

	public static ClientShootKeyNotifyHandler get() {
		return INSTANCE;
	}

	public void handle(ShootKeyNotify message) {
		send(new FireballCastRequest(message.getPosition()));
	}

}
