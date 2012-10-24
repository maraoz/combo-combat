package ar.com.game.network.message.chat;

import ar.com.game.network.BaseMessage;

public class ChatRequest extends BaseMessage {

	private final String text;

	public String getText() {
		return text;
	}

	public ChatRequest(String text) {
		this.text = text;
	}

}
