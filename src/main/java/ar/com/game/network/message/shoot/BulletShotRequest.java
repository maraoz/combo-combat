package ar.com.game.network.message.shoot;

import ar.com.game.backend.domain.Vector2D;
import ar.com.game.network.BaseMessage;

public class BulletShotRequest extends BaseMessage {

	private final Vector2D target;

	public BulletShotRequest(Vector2D target) {
		super();
		this.target = target;
	}

	public Vector2D getTarget() {
		return target;
	}

}
