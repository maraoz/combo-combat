package ar.com.nuchon.network.message.spells;

import ar.com.nuchon.backend.domain.Vector2D;
import ar.com.nuchon.network.BaseMessage;

public class FireballCastRequest extends BaseMessage {

	private final Vector2D target;

	public FireballCastRequest(Vector2D target) {
		super();
		this.target = target;
	}

	public Vector2D getTarget() {
		return target;
	}

}
