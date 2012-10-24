package ar.com.game.network.dispatch;

import java.util.Map;

import ar.com.game.network.BaseMessage;

import com.google.common.base.Preconditions;
import com.google.common.collect.Maps;

public class MessageHub {
	
	private static Map<Class<? extends BaseMessage>, MessageDispatcher<? extends BaseMessage>> dispatchers =
			Maps.newHashMap();
	
	public static <T extends BaseMessage> void route(T message) {
		@SuppressWarnings("unchecked")
		MessageDispatcher<T> dispatcher = (MessageDispatcher<T>) dispatchers.get(message.getClass());
		Preconditions.checkState(dispatcher != null, 
				"No listeners registered for message type "+message.getClass().getSimpleName());
		dispatcher.dispatch(message);
	}

	public static <T extends BaseMessage> void subscribe(Class<T> type, MessageListener<T> listener) {
		@SuppressWarnings("unchecked")
		MessageDispatcher<T> dispatcher = (MessageDispatcher<T>) dispatchers.get(type);
		
		if (dispatcher == null) {
			dispatcher = new MessageDispatcher<T>();
			dispatchers.put(type, dispatcher);
		}
		dispatcher.register(listener);
	}

}
