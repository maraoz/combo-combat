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
	
	
	public void addNewObject(GameObject object) {
		this.spawnedObjects.add(object);
	}
	
	public void addObjectChange(GameObjectDelta delta) {
		
	}
	
	
}
