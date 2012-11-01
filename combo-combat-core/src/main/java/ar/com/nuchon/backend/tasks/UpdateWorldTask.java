package ar.com.nuchon.backend.tasks;

import ar.com.nuchon.backend.ServerBackend;
import ar.com.nuchon.backend.domain.events.BulletHitEvent;
import ar.com.nuchon.network.dispatch.MessageHub;
import ar.com.nuchon.network.message.event.BulletHitNotify;



public class UpdateWorldTask extends SynchedTask implements Runnable {

	public void run() {
		
		while (true) {
			try {
				ServerBackend.update();
				for (BulletHitEvent hit : ServerBackend.checkCollisions()) {
					MessageHub.route(new BulletHitNotify(hit));
				}
				waitFPS();
			} catch (Exception e) {
				e.printStackTrace();
			}
		}

	}

}
