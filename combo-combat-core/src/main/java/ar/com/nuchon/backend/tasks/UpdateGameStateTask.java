package ar.com.nuchon.backend.tasks;

import ar.com.nuchon.backend.ServerBackend;
import ar.com.nuchon.network.dispatch.MessageHub;
import ar.com.nuchon.network.message.event.GameStateNotify;



public class UpdateGameStateTask extends SynchedTask implements Runnable {
	
	public void run() {
		
		while (true) {
			try {
				ServerBackend.update();
				MessageHub.route(new GameStateNotify(ServerBackend.getState()));
				waitFPS();
			} catch (Exception e) {
				e.printStackTrace();
			}
		}

	}

}
