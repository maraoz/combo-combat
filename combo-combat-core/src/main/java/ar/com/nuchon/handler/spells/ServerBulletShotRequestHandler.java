package ar.com.nuchon.handler.spells;

import ar.com.nuchon.backend.ServerBackend;
import ar.com.nuchon.backend.domain.Fireball;
import ar.com.nuchon.backend.domain.PlayerAvatar;
import ar.com.nuchon.handler.base.BaseServerHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.spells.FireballCastRequest;
import ar.com.nuchon.network.message.update.UpdatableCreateNotify;

public class ServerBulletShotRequestHandler extends BaseServerHandler implements
		MessageListener<FireballCastRequest> {

	private static final ServerBulletShotRequestHandler INSTANCE = new ServerBulletShotRequestHandler();

	public static ServerBulletShotRequestHandler get() {
		return INSTANCE;
	}

	private ServerBulletShotRequestHandler() {
	}

	public void handle(FireballCastRequest message) {
		Long sessionId = getClient(message.getChannel());
		PlayerAvatar player = ServerBackend.getPlayer(sessionId);
		Fireball bulletShot = ServerBackend.bulletShot(player.getPosition(), message.getTarget());
		
		sendToAll(new UpdatableCreateNotify(bulletShot));
	}

}
