package ar.com.game.handler.event;

import ar.com.game.backend.ClientBackend;
import ar.com.game.handler.base.BaseClientHandler;
import ar.com.game.network.dispatch.MessageListener;
import ar.com.game.network.message.event.BulletHitEvent;

public class ClientBulletHitEventHandler extends BaseClientHandler implements
		MessageListener<BulletHitEvent> {

	private static final ClientBulletHitEventHandler INSTANCE = new ClientBulletHitEventHandler();

	private ClientBulletHitEventHandler() {
	}

	public static ClientBulletHitEventHandler get() {
		return INSTANCE;
	}

	public void handle(BulletHitEvent message) {
		ClientBackend.bulletHit(message.getHit());
	}

}
