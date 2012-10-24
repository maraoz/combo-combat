package ar.com.game.handler.gui;

import ar.com.game.handler.base.BaseClientHandler;
import ar.com.game.network.dispatch.MessageListener;
import ar.com.game.network.message.gui.ShootKeyNotify;
import ar.com.game.network.message.shoot.BulletShotRequest;

public class ClientShootKeyNotifyHandler extends BaseClientHandler implements
		MessageListener<ShootKeyNotify> {

	private static final ClientShootKeyNotifyHandler INSTANCE = new ClientShootKeyNotifyHandler();

	private ClientShootKeyNotifyHandler() {
	}

	public static ClientShootKeyNotifyHandler get() {
		return INSTANCE;
	}

	public void handle(ShootKeyNotify message) {
		send(new BulletShotRequest(message.getPosition()));
	}

}
