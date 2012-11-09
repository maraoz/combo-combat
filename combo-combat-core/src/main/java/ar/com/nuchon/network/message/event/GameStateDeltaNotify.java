package ar.com.nuchon.network.message.event;

import ar.com.nuchon.backend.domain.base.GameStateDelta;
import ar.com.nuchon.network.BaseMessage;

public class GameStateDeltaNotify extends BaseMessage {

	private final GameStateDelta delta;

	public GameStateDeltaNotify(GameStateDelta delta) {
		super();
		this.delta = delta;
		System.out.println(delta);
	}
	
	public GameStateDelta getDelta() {
		return delta;
	}
	
}
