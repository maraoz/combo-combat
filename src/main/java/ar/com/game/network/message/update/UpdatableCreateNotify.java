package ar.com.game.network.message.update;

import ar.com.game.backend.domain.Updatable;
import ar.com.game.network.BaseMessage;

public class UpdatableCreateNotify extends BaseMessage {

	private final Updatable u;

	public UpdatableCreateNotify(Updatable u) {
		super();
		this.u = u;
	}

	public Updatable getUpdatable() {
		return u;
	}
	
}
