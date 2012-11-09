package ar.com.nuchon.network.message.event;

import ar.com.nuchon.backend.domain.base.GameState;
import ar.com.nuchon.network.BaseMessage;

public class GameStateNotify extends BaseMessage {

	private final GameState state;

	public GameStateNotify(GameState state) {
		super();
		this.state = state;
	}
	
	public GameState getState() {
		return state;
	}
	
}
