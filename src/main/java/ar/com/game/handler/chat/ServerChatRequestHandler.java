package ar.com.game.handler.chat;

import ar.com.game.handler.base.BaseServerHandler;
import ar.com.game.network.dispatch.MessageListener;
import ar.com.game.network.message.chat.ChatNotify;
import ar.com.game.network.message.chat.ChatRequest;

public class ServerChatRequestHandler extends BaseServerHandler implements MessageListener<ChatRequest> {

	private static final ServerChatRequestHandler INSTANCE = new ServerChatRequestHandler();
	
	private ServerChatRequestHandler() {
	}
	public static ServerChatRequestHandler get() {
		return INSTANCE;
	}
	
	public void handle(ChatRequest message) {
		Long sender = getClient(message.getChannel());
		sendToAll(new ChatNotify(sender.toString(), message.getText()));
		
	}

}
