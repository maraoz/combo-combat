package ar.com.game.handler.base;

import java.util.Collection;

import org.jboss.netty.channel.Channel;
import org.jboss.netty.channel.ChannelFuture;
import org.jboss.netty.channel.ChannelFutureListener;

import ar.com.game.network.BaseMessage;

public class BaseServerHandler {

	private static ServerConnectionManager connectionManager = new ServerConnectionManager();

	public Long addClient(Channel channel) {
		return connectionManager.addClient(channel);
	}
	public void removeClient(Long session) {
		connectionManager.removeClient(session);
	}
	public Long getClient(Channel channel) {
		return connectionManager.getSessionFor(channel);
	}

	public void send(BaseMessage message, Long destSession) {
		connectionManager.getChannelFor(destSession).write(message);
	}
	
	public void sendAndTerminateConnection(BaseMessage message, Long destSession) {
		ChannelFuture f = connectionManager.getChannelFor(destSession).write(message);
		f.addListener(ChannelFutureListener.CLOSE);
	}

	public void send(BaseMessage message, Collection<Long> sessions) {
		for (Long session : sessions) {
			send(message, session);
		}
	}

	public void sendToAll(BaseMessage message) {
		send(message, connectionManager.getAll());
	}
}
