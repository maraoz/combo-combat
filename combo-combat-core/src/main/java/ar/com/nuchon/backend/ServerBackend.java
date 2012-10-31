package ar.com.nuchon.backend;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.List;
import java.util.Map;

import ar.com.nuchon.backend.domain.Bullet;
import ar.com.nuchon.backend.domain.BulletHit;
import ar.com.nuchon.backend.domain.PlayerAvatar;
import ar.com.nuchon.backend.domain.Updatable;
import ar.com.nuchon.backend.domain.Vector2D;

import com.google.common.base.Preconditions;
import com.google.common.collect.Lists;
import com.google.common.collect.Maps;

public class ServerBackend {
	
	private static final float HIT_RANGE = 15;
	private static Map<Long, PlayerAvatar> players = Maps.newHashMap();
	private static List<Updatable> updatees = Collections.synchronizedList(new ArrayList<Updatable>());
	private static List<Bullet> bullets = Collections.synchronizedList(new ArrayList<Bullet>());
	
	
	public static void movePlayer(Long id, Vector2D delta) {
		Preconditions.checkNotNull(id);
		Preconditions.checkNotNull(delta);
		
		PlayerAvatar player = getPlayer(id);
		Vector2D moved = player.getPosition().plus(delta);
		player.setPosition(moved);
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
	
	public static Bullet bulletShot(Vector2D origin, Vector2D target) {
		Bullet bullet = new Bullet(origin, target);
		bullets.add(bullet);
		updatees.add(bullet);
		return bullet;
	}
	
	public static List<BulletHit> checkCollisions() {
		List<BulletHit> ret = Lists.newArrayList();
		for (PlayerAvatar p : players.values()) {
			synchronized (bullets) {
				List<Bullet> toRemove = Lists.newArrayList();
				for (Bullet b : bullets) {
					if (b.getPos().distanceTo(p.getPosition()) < HIT_RANGE) {
						p.reduceHP();
						toRemove.add(b);
						ret.add(new BulletHit(p.getId(), b.getId()));
					}
				}
				bullets.removeAll(toRemove);
			}
		}
		return ret;
	}
	
	public static void update() {
		synchronized (updatees) {
			for (Updatable u : updatees) {
				u.update();
			}
		}
	}

}
