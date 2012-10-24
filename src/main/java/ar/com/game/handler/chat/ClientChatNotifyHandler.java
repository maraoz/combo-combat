package ar.com.game.handler.chat;

import ar.com.game.handler.base.BaseClientHandler;
import ar.com.game.network.dispatch.MessageListener;
import ar.com.game.network.message.chat.ChatNotify;

public class ClientChatNotifyHandler extends BaseClientHandler implements MessageListener<ChatNotify> {

	private static final ClientChatNotifyHandler INSTANCE = new ClientChatNotifyHandler();

	public static ClientChatNotifyHandler get() {
		return INSTANCE;
	}

	private ClientChatNotifyHandler() {
	}

	public void handle(ChatNotify message) {
		System.out.println(message.getWho() + ": " + message.getText());

	}
}
