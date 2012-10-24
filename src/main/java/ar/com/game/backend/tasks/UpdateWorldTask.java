package ar.com.game.backend.tasks;

import ar.com.game.backend.ServerBackend;
import ar.com.game.backend.domain.BulletHit;
import ar.com.game.network.dispatch.MessageHub;
import ar.com.game.network.message.event.BulletHitEvent;



public class UpdateWorldTask extends SynchedTask implements Runnable {

	public void run() {
		while (true) {
			ServerBackend.update();
			for (BulletHit hit : ServerBackend.checkCollisions()) {
				MessageHub.route(new BulletHitEvent(hit));
			}
			waitFPS();
		}

	}

}
