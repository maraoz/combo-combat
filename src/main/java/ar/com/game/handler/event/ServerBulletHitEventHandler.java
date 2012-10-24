package ar.com.game.handler.event;

import ar.com.game.handler.base.BaseServerHandler;
import ar.com.game.network.dispatch.MessageListener;
import ar.com.game.network.message.event.BulletHitEvent;

public class ServerBulletHitEventHandler extends BaseServerHandler implements
		MessageListener<BulletHitEvent> {

	private static final ServerBulletHitEventHandler INSTANCE = new ServerBulletHitEventHandler();

	private ServerBulletHitEventHandler() {
	}

	public static ServerBulletHitEventHandler get() {
		return INSTANCE;
	}

	public void handle(BulletHitEvent message) {
		sendToAll(message);
	}

}
