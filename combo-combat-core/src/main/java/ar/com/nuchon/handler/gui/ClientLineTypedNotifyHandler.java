package ar.com.nuchon.handler.gui;

import ar.com.nuchon.handler.base.BaseClientHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.chat.ChatRequest;
import ar.com.nuchon.network.message.gui.LineTypedNotify;

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
