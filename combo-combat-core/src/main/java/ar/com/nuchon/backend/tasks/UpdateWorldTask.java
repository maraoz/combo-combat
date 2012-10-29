package ar.com.nuchon.backend.tasks;

import ar.com.nuchon.backend.ServerBackend;
import ar.com.nuchon.backend.domain.BulletHit;
import ar.com.nuchon.network.dispatch.MessageHub;
import ar.com.nuchon.network.message.event.BulletHitEvent;



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
