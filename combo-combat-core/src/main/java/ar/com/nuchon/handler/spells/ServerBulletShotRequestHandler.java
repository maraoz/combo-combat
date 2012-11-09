package ar.com.nuchon.handler.spells;

import ar.com.nuchon.backend.ServerBackend;
import ar.com.nuchon.backend.domain.PlayerAvatar;
import ar.com.nuchon.handler.base.BaseServerHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.spells.FireballCastRequest;

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
		ServerBackend.bulletShot(player.getPosition(), message.getTarget());
		
	}

}
