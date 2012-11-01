package ar.com.nuchon.network.message.spells;

import ar.com.nuchon.backend.domain.Vector2D;
import ar.com.nuchon.network.BaseMessage;

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
