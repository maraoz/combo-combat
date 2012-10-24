package ar.com.game.backend.domain;

import java.io.Serializable;

public interface Updatable extends Serializable {
	
	public void update();
	
	public boolean isAlive();
	
}
