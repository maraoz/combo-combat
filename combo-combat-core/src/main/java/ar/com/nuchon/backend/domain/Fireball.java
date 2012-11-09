package ar.com.nuchon.backend.domain;

import ar.com.nuchon.annotation.NetworkData;
import ar.com.nuchon.backend.domain.base.GameObject;


public class Fireball extends GameObject implements Updatable {

	private static final int VELOCITY = 5;
	
	@NetworkData
	private Vector2D pos;
	@NetworkData
	private Vector2D speed;
	
	public Fireball(Vector2D pos, Vector2D target) {
		super();
		this.speed = pos.directionTo(target).times(VELOCITY);
		this.pos = pos.plus(speed.times(3));
	}
	public Vector2D getPos() {
		return pos;
	}
	public Vector2D getSpeed() {
		return speed;
	} 
	
	public void update() {
		this.pos.add(speed);
	}
	@Override
	public String toString() {
		return "Fireball [pos=" + pos + ", speed=" + speed + "]";
	}
}
