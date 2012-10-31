package ar.com.nuchon.backend.tasks;

import java.util.List;

import ar.com.nuchon.backend.domain.Vector2D;
import ar.com.nuchon.network.dispatch.MessageHub;
import ar.com.nuchon.network.message.gui.MoveKeyNotify;

import com.google.common.collect.Lists;

public class MageBotTask extends SynchedTask implements Runnable {

	
	private static List<Vector2D> deltas = Lists.newArrayList(Vector2D.UP, Vector2D.LEFT, Vector2D.DOWN, Vector2D.RIGHT);
	
	
	public void run() {

		sleep();
		for (int i = 0; i < 300; i++) {
			MessageHub.route(new MoveKeyNotify(new Vector2D(1, 1)));
			sleep(50);
		}

		while (true) {
			for (Vector2D delta : deltas) {
				for (int step=0; step<100; step++) {
					MessageHub.route(new MoveKeyNotify(delta));
					sleep(50);
				}
				sleep();
			}
		}

	}

	private void sleep(long n) {
		try {
			Thread.sleep(n);
		} catch (InterruptedException e) {
		}
	}
	
	private void sleep() {
		sleep(500);
	}

}
