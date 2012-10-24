package ar.com.game.handler.shoot;

import ar.com.game.backend.ServerBackend;
import ar.com.game.backend.domain.Bullet;
import ar.com.game.backend.domain.PlayerAvatar;
import ar.com.game.handler.base.BaseServerHandler;
import ar.com.game.network.dispatch.MessageListener;
import ar.com.game.network.message.shoot.BulletShotRequest;
import ar.com.game.network.message.update.UpdatableCreateNotify;

public class ServerBulletShotRequestHandler extends BaseServerHandler implements
		MessageListener<BulletShotRequest> {

	private static final ServerBulletShotRequestHandler INSTANCE = new ServerBulletShotRequestHandler();

	public static ServerBulletShotRequestHandler get() {
		return INSTANCE;
	}

	private ServerBulletShotRequestHandler() {
	}

	public void handle(BulletShotRequest message) {
		Long sessionId = getClient(message.getChannel());
		PlayerAvatar player = ServerBackend.getPlayer(sessionId);
		Bullet bulletShot = ServerBackend.bulletShot(player.getPosition(), message.getTarget());
		
		sendToAll(new UpdatableCreateNotify(bulletShot));
	}

}
