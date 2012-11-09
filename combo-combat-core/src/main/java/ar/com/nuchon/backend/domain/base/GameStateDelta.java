package ar.com.nuchon.backend.domain.base;

import java.io.Serializable;
import java.util.List;

import com.google.common.collect.Lists;

/**
 * Represents all the changes needed to go from one GameState to another
 * @author maraoz
 *
 */
public class GameStateDelta implements Serializable {

	
	private List<GameObjectDelta> objectChanges = Lists.newArrayList();
	private List<GameObject> spawnedObjects = Lists.newArrayList();
	private final Long sequence;
	
	public GameStateDelta(Long sequence) {
		super();
		this.sequence = sequence;
	}

	public void addNewObject(GameObject object) {
		this.spawnedObjects.add(object);
	}
	
	public void addObjectChange(GameObjectDelta delta) {
		objectChanges.add(delta);
	}

	@Override
	public String toString() {
		return "GameStateDelta [objectChanges=" + objectChanges
				+ ", spawnedObjects=" + spawnedObjects + ", sequence="
				+ sequence + "]";
	}

	public long getSequence() {
		return sequence;
	}
	
	
	
}
