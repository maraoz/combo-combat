package ar.com.nuchon.backend.domain.base;

import java.util.Set;

import com.google.common.base.Predicate;
import com.google.common.collect.Sets;


public class GameState {
	
	private final long sequence;
	private Set<GameObject> objects;

	public GameState(long sequence) {
		super();
		this.sequence = sequence;
	}
	
	public Set<GameObject> getGameObjects() {
		return objects;
	}
	
	
	/**
	 * Creates the delta that would take this state to the newer state 
	 */
	public GameStateDelta getDeltaTo(GameState newer) {
		// TODO
		GameStateDelta delta = new GameStateDelta();
		
		Set<GameObject> dissapeared = Sets.difference(this.getGameObjects(), newer.getGameObjects());
		Set<GameObject> appeared = Sets.difference(newer.getGameObjects(), this.getGameObjects());
		Set<GameObject> continued = Sets.intersection(this.getGameObjects(), newer.getGameObjects());
		
		// objects existing in this state and not existing in new state
		for (GameObject gameObject : dissapeared) {
			delta.addObjectChange(GameObjectDelta.destroyDeltaFor(gameObject));
		}
		// objects not existing in this state and existing in new state
		for (GameObject gameObject : appeared) {
			delta.addNewObject(gameObject);
		}
		// objects which may have suffered changes in their fields
		for (GameObject gameObject : continued) {
			// TODO
		}
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
