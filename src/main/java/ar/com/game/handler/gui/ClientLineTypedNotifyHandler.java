package ar.com.game.handler.gui;

import ar.com.game.handler.base.BaseClientHandler;
import ar.com.game.network.dispatch.MessageListener;
import ar.com.game.network.message.chat.ChatRequest;
import ar.com.game.network.message.gui.LineTypedNotify;

public class ClientLineTypedNotifyHandler extends BaseClientHandler implements
		MessageListener<LineTypedNotify> {

	private static final ClientLineTypedNotifyHandler INSTANCE = new ClientLineTypedNotifyHandler();

	private ClientLineTypedNotifyHandler() {
	}

	public static ClientLineTypedNotifyHandler get() {
		return INSTANCE;
	}

	public void handle(LineTypedNotify message) {
		send(new ChatRequest(message.getLine()));
	}

}
