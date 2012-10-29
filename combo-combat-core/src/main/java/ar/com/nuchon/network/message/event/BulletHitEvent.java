package ar.com.nuchon.network.message.event;

import ar.com.nuchon.backend.domain.BulletHit;
import ar.com.nuchon.network.BaseMessage;

public class BulletHitEvent extends BaseMessage {

	private final BulletHit hit;

	public BulletHitEvent(BulletHit hit) {
		super();
		this.hit = hit;
	}

	public BulletHit getHit() {
		return hit;
	}

}
