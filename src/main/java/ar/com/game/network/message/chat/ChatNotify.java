package ar.com.game.network.message.chat;

import ar.com.game.network.BaseMessage;

public class ChatNotify extends BaseMessage {

	private final String who;
	private final String text;

	public String getText() {
		return text;
	}

	public String getWho() {
		return who;
	}

	public ChatNotify(String who, String text) {
		super();
		this.who = who;
		this.text = text;
	}

}
