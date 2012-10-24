package ar.com.nuchon.backend.tasks;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

import ar.com.nuchon.network.dispatch.MessageHub;
import ar.com.nuchon.network.message.gui.LineTypedNotify;

public class LineReaderTask implements Runnable {

	public void run() {
		String line = "";
		BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
		while (!line.equals("quit")) {
			try {
				line = br.readLine();
				MessageHub.route(new LineTypedNotify(line));
			} catch (IOException e) {
				e.printStackTrace();
				break;
			}
		}

	}

}
