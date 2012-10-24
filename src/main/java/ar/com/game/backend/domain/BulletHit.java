package ar.com.game.backend.domain;

import java.io.Serializable;

public class BulletHit implements Serializable {

	private final Long victimId;
	private final Long bulletId;
	
	public BulletHit(Long victimId, Long bulletId) {
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
