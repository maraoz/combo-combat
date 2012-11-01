package ar.com.nuchon.network.message.event;

import ar.com.nuchon.backend.domain.events.BulletHitEvent;
import ar.com.nuchon.network.BaseMessage;

public class BulletHitNotify extends BaseMessage {

	private final BulletHitEvent hit;

	public BulletHitNotify(BulletHitEvent hit) {
		super();
		this.hit = hit;
	}

	public BulletHitEvent getHit() {
		return hit;
	}

}
