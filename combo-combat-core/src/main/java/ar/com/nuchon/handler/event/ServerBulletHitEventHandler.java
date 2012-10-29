package ar.com.nuchon.handler.event;

import ar.com.nuchon.handler.base.BaseServerHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.event.BulletHitEvent;

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
