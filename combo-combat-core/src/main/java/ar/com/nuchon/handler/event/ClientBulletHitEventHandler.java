package ar.com.nuchon.handler.event;

import ar.com.nuchon.backend.ClientBackend;
import ar.com.nuchon.handler.base.BaseClientHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.event.BulletHitNotify;

public class ClientBulletHitEventHandler extends BaseClientHandler implements
		MessageListener<BulletHitNotify> {

	private static final ClientBulletHitEventHandler INSTANCE = new ClientBulletHitEventHandler();

	private ClientBulletHitEventHandler() {
	}

	public static ClientBulletHitEventHandler get() {
		return INSTANCE;
	}

	public void handle(BulletHitNotify message) {
		ClientBackend.bulletHit(message.getHit());
	}

}
