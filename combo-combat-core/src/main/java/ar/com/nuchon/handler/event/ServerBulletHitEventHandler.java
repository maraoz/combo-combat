package ar.com.nuchon.handler.event;

import ar.com.nuchon.handler.base.BaseServerHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.event.BulletHitNotify;

public class ServerBulletHitEventHandler extends BaseServerHandler implements
		MessageListener<BulletHitNotify> {

	private static final ServerBulletHitEventHandler INSTANCE = new ServerBulletHitEventHandler();

	private ServerBulletHitEventHandler() {
	}

	public static ServerBulletHitEventHandler get() {
		return INSTANCE;
	}

	public void handle(BulletHitNotify message) {
		sendToAll(message);
	}

}
