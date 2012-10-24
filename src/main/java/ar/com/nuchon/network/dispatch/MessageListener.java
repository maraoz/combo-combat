package ar.com.nuchon.network.dispatch;

import ar.com.nuchon.network.BaseMessage;


public interface MessageListener<T extends BaseMessage> {

	public void handle(T message);
	
}
