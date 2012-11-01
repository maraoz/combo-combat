package ar.com.nuchon.backend.domain;

public class GameState {
	
	private final long sequence;

	public GameState(long sequence) {
		super();
		this.sequence = sequence;
	}
	
	public GameStateDelta deltaTo(GameState other) {
		return null;
	}
	
}
