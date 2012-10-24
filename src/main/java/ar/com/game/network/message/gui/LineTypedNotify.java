package ar.com.game.network.message.gui;

import ar.com.game.network.BaseMessage;

public class LineTypedNotify extends BaseMessage {

	private final String line;

	public LineTypedNotify(String line) {
		super();
		this.line = line;
	}

	public String getLine() {
		return line;
	}

}
