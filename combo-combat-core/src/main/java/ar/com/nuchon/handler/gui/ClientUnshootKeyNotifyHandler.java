package ar.com.nuchon.handler.gui;

import ar.com.nuchon.handler.base.BaseClientHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.gui.ShootKeyNotify;
import ar.com.nuchon.network.message.spells.BulletUnshotRequest;

public class ClientUnshootKeyNotifyHandler extends BaseClientHandler implements
		MessageListener<ShootKeyNotify> {

	private static final ClientUnshootKeyNotifyHandler INSTANCE = new ClientUnshootKeyNotifyHandler();

	private ClientUnshootKeyNotifyHandler() {
	}

	public static ClientUnshootKeyNotifyHandler get() {
		return INSTANCE;
	}

	public void handle(ShootKeyNotify message) {
		send(new BulletUnshotRequest());
	}

}
