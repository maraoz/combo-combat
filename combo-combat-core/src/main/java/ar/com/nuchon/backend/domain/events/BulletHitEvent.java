package ar.com.nuchon.backend.domain.events;

import java.io.Serializable;

public class BulletHitEvent implements Serializable {

	private final Long victimId;
	private final Long bulletId;
	
	public BulletHitEvent(Long victimId, Long bulletId) {
		super();
		this.victimId = victimId;
		this.bulletId = bulletId;
	}

	public Long getVictimId() {
		return victimId;
	}

	public Long getBulletId() {
		return bulletId;
	}
	
	
	
}
