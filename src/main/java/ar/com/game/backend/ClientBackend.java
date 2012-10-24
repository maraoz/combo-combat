package ar.com.game.backend;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.List;
import java.util.Map;

import ar.com.game.backend.domain.Bullet;
import ar.com.game.backend.domain.BulletHit;
import ar.com.game.backend.domain.Updatable;
import ar.com.game.backend.domain.Vector2D;

import com.google.common.collect.Lists;
import com.google.common.collect.Maps;

public class ClientBackend {
	
	private static Map<Long, Vector2D> playerPositions = Maps.newHashMap();
	private static List<Updatable> updatees = Collections.synchronizedList(new ArrayList<Updatable>());
	
	public static void movePlayer(Long id, Vector2D where) {
		playerPositions.put(id, where);
	}
	
	public static void removePlayer(Long id) {
		playerPositions.remove(id);
	}
	
	public static Collection<Vector2D> getPositions() {
		return playerPositions.values();
	}
	
	public static void addUpdatable(Updatable u) {
		updatees.add(u);
	}
	
	public static List<Bullet> getBullets() {
		List<Bullet> ret = Lists.newArrayList();
		synchronized (updatees) {
			for (Updatable u : updatees) {
				ret.add((Bullet) u);
			}
		}
		return ret;
	}
	
	public static void update() {
		synchronized (updatees) {
			for (Updatable u : updatees) {
				if (!u.isAlive()) {
					updatees.remove(u);
				}
				u.update();
			}
		}
	}

	public static void bulletHit(BulletHit hit) {
		// do something! boom
		System.out.println("bullet "+hit.getBulletId()+" hit player "+hit.getVictimId());
	}

}
