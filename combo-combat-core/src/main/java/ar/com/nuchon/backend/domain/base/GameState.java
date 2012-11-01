package ar.com.nuchon.backend.domain.base;


public class GameState {
	
	private final long sequence;

	public GameState(long sequence) {
		super();
		this.sequence = sequence;
	}
	
	
	/**
	 * Creates the delta that would take this state to the other state 
	 */
	public GameStateDelta getDeltaTo(GameState other) {
		// TODO
		return null;
	}
	
	/**
	 * Returns a new state with the delta changes applied to this state
	 */
	public GameState applyDelta(GameStateDelta delta) {
		// TODO
		return null;
	}
	
}
