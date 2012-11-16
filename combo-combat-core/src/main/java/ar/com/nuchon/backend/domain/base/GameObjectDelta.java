package ar.com.nuchon.backend.domain.base;

import java.util.List;

import com.google.common.collect.Lists;

/**
 * Represents changes needed to be done on a game object
 * @author maraoz
 *
 */
public class GameObjectDelta {
	
	private Class<? extends GameObject> type;
	private long objectId;
	private final List<GameObjectFieldChange> changes = Lists.newArrayList();
	
	
	
	public GameObjectDelta(GameObject o) {
		this.type = o.getClass();
		this.objectId = o.getId();
	}
	
	public void addFieldChange(GameObjectFieldChange fieldChange) {
		changes.add(fieldChange);
	}
	
	public List<GameObjectFieldChange> getChanges() {
		return changes;
	}
	
	public long getObjectId() {
		return objectId;
	}
	
	public Class<? extends GameObject> getType() {
		return type;
	}
	
	@Override
	public String toString() {
		return "GameObjectDelta [" + type + "(" + objectId
				+ ")->" + changes + "]";
	}

	// factory method for a delta representing game object must be destroyed
	public static GameObjectDelta destroyDeltaFor(GameObject object) {
		GameObjectDelta ret = new GameObjectDelta(object);
		ret.addFieldChange(new GameObjectFieldChange("destroyMe", true));
		return ret;
	}
	
	

}