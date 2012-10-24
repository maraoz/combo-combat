package ar.com.game.network.message.move;

import ar.com.game.backend.domain.Vector2D;
import ar.com.game.network.BaseMessage;

public class PlayerMoveNotify extends BaseMessage {

	private final Long who;
	private final Vector2D where;

	public PlayerMoveNotify(Long who, Vector2D where) {
		super();
		this.who = who;
		this.where = where;
	}

	public Long getWho() {
		return who;
	}

	public Vector2D getWhere() {
		return where;
	}

}
