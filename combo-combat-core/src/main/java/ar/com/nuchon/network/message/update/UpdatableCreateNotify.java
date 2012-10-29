package ar.com.nuchon.network.message.update;

import ar.com.nuchon.backend.domain.Updatable;
import ar.com.nuchon.network.BaseMessage;

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
