package ar.com.nuchon.backend;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.List;
import java.util.Map;

import ar.com.nuchon.backend.domain.Fireball;
import ar.com.nuchon.backend.domain.Updatable;
import ar.com.nuchon.backend.domain.Vector2D;
import ar.com.nuchon.backend.domain.events.BulletHitEvent;

import com.google.common.base.Predicate;
import com.google.common.collect.Collections2;
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
	
	public static List<Fireball> getBullets() {
		List<Fireball> ret = Lists.newArrayList();
		synchronized (updatees) {
			for (Updatable u : updatees) {
				ret.add((Fireball) u);
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

	public static void bulletHit(final BulletHitEvent hit) {
		// do something! boom
		System.out.println("bullet "+hit.getBulletId()+" hit player "+hit.getVictimId());
		updatees.removeAll(Collections2.filter(getBullets(), new Predicate<Fireball>() {
			public boolean apply(Fireball x) {
				return x.getId().equals(hit.getBulletId());
			}
		}));
	}

}
