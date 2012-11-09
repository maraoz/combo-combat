package ar.com.nuchon.backend.domain.base;

import java.lang.reflect.Field;
import java.util.Map;
import java.util.Set;

import ar.com.nuchon.annotation.GameObjectInspector;

import com.google.common.base.Function;
import com.google.common.base.Preconditions;
import com.google.common.collect.Collections2;
import com.google.common.collect.Maps;
import com.google.common.collect.Sets;


public class GameState {
	
	// sequence number of game state in chronological order.
	private final long sequence;
	private final Map<Long, GameObject> objects;

	public GameState() {
		this.objects = Maps.newHashMap();
		this.sequence = 0;
	}
	
	/**
	 * Copy constructor
	 */
	public GameState(GameState other) {
		this.objects = Maps.newHashMap(other.objects);
		this.sequence = other.sequence + 1;
	}
	
	public long getSequence() {
		return sequence;
	}
	
	public Set<GameObject> getGameObjects() {
		return Sets.newHashSet(objects.values());
	}
	
	public GameObject getGameObject(Long id) {
		return this.objects.get(id);
	}
	
	public Set<Long> getGameObjectIds() {
		return Sets.newHashSet(Collections2.transform(getGameObjects(), new Function<GameObject, Long>() {
			public Long apply(GameObject input) {
				return input.getId();
			}
		}));
	}
	
	
	/**
	 * Creates the delta that would take this state to the newer state 
	 */
	public GameStateDelta getDeltaTo(GameState newer) {
		GameStateDelta delta = new GameStateDelta();
		
		Set<GameObject> dissapeared = Sets.difference(this.getGameObjects(), newer.getGameObjects());
		Set<GameObject> appeared = Sets.difference(newer.getGameObjects(), this.getGameObjects());
		Set<Long> continued = Sets.intersection(this.getGameObjectIds(), newer.getGameObjectIds());
		
		// objects existing in this state and not existing in new state
		for (GameObject gameObject : dissapeared) {
			delta.addObjectChange(GameObjectDelta.destroyDeltaFor(gameObject));
		}
		// objects not existing in this state and existing in new state
		for (GameObject gameObject : appeared) {
			delta.addNewObject(gameObject);
		}
		// objects which may have suffered changes in their fields
		for (Long id: continued) {
			GameObject current = this.getGameObject(id);
			GameObject future = newer.getGameObject(id);
			Preconditions.checkNotNull(current);
			Preconditions.checkNotNull(future);
			Class<? extends GameObject> type = current.getClass();
			Set<Field> networkedFields = GameObjectInspector.getNetworkedFields(type);
			boolean changed = false;
			GameObjectDelta objectDelta = new GameObjectDelta(type, id);
			for (Field f : networkedFields) {
				Object currentValue;
				Object futureValue;
				try {
					currentValue = f.get(current);
					futureValue = f.get(future);
					if (!currentValue.equals(futureValue)) {
						changed = true;
						objectDelta.addFieldChange(new GameObjectFieldChange(f.getName(), futureValue));
					}
				} catch (IllegalArgumentException e) {
					e.printStackTrace();
				} catch (IllegalAccessException e) {
					e.printStackTrace();
				}
			}
			if (changed) {
				delta.addObjectChange(objectDelta);
			}
			
		}
		return delta;
	}
	
	/**
	 * Returns a new state with the delta changes applied to this state
	 */
	public GameState applyDelta(GameStateDelta delta) {
		GameState changedState = new GameState(this);
		// TODO
		return changedState;
	}
	
}
