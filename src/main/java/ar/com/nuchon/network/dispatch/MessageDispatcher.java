package ar.com.nuchon.network.dispatch;

import java.util.Set;

import ar.com.nuchon.network.BaseMessage;

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
