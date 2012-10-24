package ar.com.game.network.dispatch;

import java.util.Set;

import ar.com.game.network.BaseMessage;

import com.google.common.base.Preconditions;
import com.google.common.collect.Sets;

public class MessageDispatcher<T extends BaseMessage> {

	private Set<MessageListener<T>> listeners = Sets.newHashSet();
	
	public void register(MessageListener<T> listener) {
		Preconditions.checkNotNull(listener);
		listeners.add(listener);
	}
	
	public void dispatch(T message) {
		for (MessageListener<T> listener : listeners) {
			listener.handle(message);
		}
	}
	
}
