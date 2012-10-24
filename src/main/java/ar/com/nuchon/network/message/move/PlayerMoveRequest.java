package ar.com.nuchon.network.message.move;

import ar.com.nuchon.backend.domain.Vector2D;
import ar.com.nuchon.network.BaseMessage;

public class PlayerMoveRequest extends BaseMessage {

	private final Vector2D pos;

	public PlayerMoveRequest(Vector2D pos) {
		super();
		this.pos = pos;
	}

	public Vector2D getDelta() {
		return pos;
	}

}
