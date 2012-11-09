package ar.com.nuchon.backend;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.List;
import java.util.Map;

import ar.com.nuchon.backend.domain.Fireball;
import ar.com.nuchon.backend.domain.PlayerAvatar;
import ar.com.nuchon.backend.domain.Updatable;
import ar.com.nuchon.backend.domain.Vector2D;
import ar.com.nuchon.backend.domain.base.GameState;

import com.google.common.base.Preconditions;
import com.google.common.collect.Lists;
import com.google.common.collect.Maps;

public class ServerBackend {
	
	public static GameState currentState = new GameState();

	private static final List<GameState> stateMemory = Lists.newLinkedList();
	private static Long latestStateId = 0L; 
	private static final Map<Long, Long> sessionToLastAckState = Maps.newHashMap();
	
	public static List<GameState> getStateMemory() {
		return stateMemory;
	}
	
	public static Long getLatestStateId() {
		return latestStateId;
	}
	
	public static Map<Long, Long> getSessionToLastAckState() {
		return sessionToLastAckState;
	}
	
	private static final float HIT_RANGE = 15;
	private static Map<Long, PlayerAvatar> players = Maps.newHashMap();
	private static List<Updatable> updatees = Collections.synchronizedList(new ArrayList<Updatable>());
	private static List<Fireball> bullets = Collections.synchronizedList(new ArrayList<Fireball>());
	
	
	public static void movePlayer(Long id, Vector2D delta) {
		Preconditions.checkNotNull(id);
		Preconditions.checkNotNull(delta);
		
		PlayerAvatar player = getPlayer(id);
		player.getPosition().add(delta);
	}
	
	public static void removePlayer(Long id) {
		players.remove(id);
	}
	
	public static Collection<PlayerAvatar> getPlayers() {
		return players.values();
	}
	
	public static Vector2D addPlayer(Long id){
		Vector2D pos = new Vector2D(50, 50);
		players.put(id, new PlayerAvatar(pos, 100.0, id));
		return pos;
	}

	public static PlayerAvatar getPlayer(Long sessionId) {
		return players.get(sessionId);
	}
	
	public static void bulletShot(Vector2D origin, Vector2D target) {
		GameState current = getState();
		Fireball fireball = new Fireball(origin, target);
		current.addObject(fireball);
		setState(current);
		
	}
	
	public static GameState getState() {
		return currentState.next();
	}
	
	public static void setState(GameState state) {
		currentState = state;
	}

	public static void update() {
		currentState.update();
	}
	
}
