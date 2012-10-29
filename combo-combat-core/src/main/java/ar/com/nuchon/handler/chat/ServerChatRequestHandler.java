package ar.com.nuchon.handler.chat;

import ar.com.nuchon.handler.base.BaseServerHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.chat.ChatNotify;
import ar.com.nuchon.network.message.chat.ChatRequest;

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
