package ar.com.nuchon.backend.domain;

import com.badlogic.gdx.math.Matrix3;
import com.badlogic.gdx.math.Vector2;

public class Vector2D extends Vector2 {
	
	public static final Vector2D UP = new Vector2D(0.0f, 1.0f);
	public static final Vector2D DOWN = new Vector2D(0.0f, -1.0f);
	public static final Vector2D RIGHT = new Vector2D(1.0f,0.0f);
	public static final Vector2D LEFT = new Vector2D(-1.0f,0.0f);
	
	public Vector2D(float x, float y) {
		super(x, y);
	}
	
	public Vector2D() {
		super();
	}
	
	public Vector2D(Vector2D v) {
		super(v);
	}
	
	@Override
	public Vector2D add(float x, float y) {
		return (Vector2D) super.add(x, y);
	}
	
	@Override
	public Vector2D add(Vector2 v) {
		return (Vector2D) super.add(v);
	}

	@Override
	public Vector2D cpy() {
		return new Vector2D(this);
	}
	
	@Override
	public Vector2D lerp(Vector2 target, float alpha) {
		return (Vector2D) super.lerp(target, alpha);
	}
	
	@Override
	public Vector2D mul(float scalar) {
		return (Vector2D) super.mul(scalar);
	}
	
	@Override
	public Vector2D mul(Matrix3 mat) {
		return (Vector2D) super.mul(mat);
	}
	
	@Override
	public Vector2D nor() {
		return (Vector2D) super.nor();
	}
	
	@Override
	public Vector2D rotate(float angle) {
		return (Vector2D) super.rotate(angle);
	}
	
	@Override
	public Vector2D set(float x, float y) {
		return (Vector2D) super.set(x, y);
	}
	
	@Override
	public Vector2D set(Vector2 v) {
		return (Vector2D) super.set(v);
	}
	
	@Override
	public Vector2D sub(float x, float y) {
		return (Vector2D) super.sub(x, y);
	}
	
	@Override
	public Vector2D sub(Vector2 v) {
		return (Vector2D) super.sub(v);
	}
	
	
	
	
	
	// ADDED METHODS
	/**
	 * Returns a new unitary vector pointing from this point to the other point 
	 */
	public Vector2D directionTo(Vector2D other) {
		Vector2D diff = other.minus(this);
		return diff.nor();
	}
	
	
	// functional version of add
	public Vector2D plus(Vector2D other) {
		return new Vector2D(x + other.x, y + other.y);
	}
	
	// functional version of sub	
	public Vector2D minus(Vector2D other) {
		return new Vector2D(x - other.x, y - other.y);
	}

	// functional version of mul
	public Vector2D times(int alpha) {
		return new Vector2D(alpha*x, alpha*y);
	}
	

}
