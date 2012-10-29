package ar.com.nuchon.network.message.move;

import ar.com.nuchon.backend.domain.Vector2D;
import ar.com.nuchon.network.BaseMessage;

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
