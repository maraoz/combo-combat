package ar.com.game.network.dispatch;

import ar.com.game.network.BaseMessage;


public interface MessageListener<T extends BaseMessage> {

	public void handle(T message);
	
}
