package ar.com.game.network.message.move;

import ar.com.game.backend.domain.Vector2D;
import ar.com.game.network.BaseMessage;

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
