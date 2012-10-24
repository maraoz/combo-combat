package ar.com.nuchon.backend.domain;

import java.io.Serializable;

public class Vector2D implements Serializable {

	private final float x, y;

	public Vector2D(float x, float y) {
		super();
		this.x = x;
		this.y = y;
	}

	public float getX() {
		return x;
	}

	public float getY() {
		return y;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		Vector2D other = (Vector2D) obj;
		if (x != other.x)
			return false;
		if (y != other.y)
			return false;
		return true;
	};
	
	@Override
	public String toString() {
		return "("+x+", "+y+")";
	}

	public Vector2D plus(Vector2D other) {
		return new Vector2D( x + other.getX(), y + other.getY());
	}
	
	public Vector2D minus(Vector2D other) {
		return new Vector2D(x - other.getX(), y - other.getY());
	}
	
	public float norm() {
		return (float) Math.sqrt(x*x + y*y);
	}
	
	public Vector2D times(float k) {
		return new Vector2D(k*getX(), k*getY());
	}
	
	public Vector2D directionTo(Vector2D other) {
		Vector2D diff = other.minus(this);
		return diff.times(1/diff.norm());
	}
	
	public float distanceTo(Vector2D other) {
		return this.minus(other).norm();
	}
	
	
}
