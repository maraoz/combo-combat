package ar.com.game.network.message.event;

import ar.com.game.backend.domain.BulletHit;
import ar.com.game.network.BaseMessage;

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
