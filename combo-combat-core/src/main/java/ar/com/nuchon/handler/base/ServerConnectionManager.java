package ar.com.nuchon.handler.base;

import java.util.Set;

import org.jboss.netty.channel.Channel;

import ar.com.nuchon.backend.domain.base.IdentityProvider;

import com.google.common.base.Preconditions;
import com.google.common.collect.BiMap;
import com.google.common.collect.HashBiMap;

public class ServerConnectionManager {

	BiMap<Long, Channel> clients = HashBiMap.create();

	/**
	 * Adds the desired client and returns his new session id. Throws
	 * IllegalStateException if client already connected
	 */
	public Long addClient(Channel channel) {
		Preconditions.checkState(!clients.containsValue(channel));
		Long sessionId = IdentityProvider.next();
		clients.put(sessionId, channel);
		return sessionId;
	}

	public Set<Long> getAll() {
		return clients.keySet();
	}

	public Channel getChannelFor(Long session) {
		return clients.get(session);
	}

	public Long getSessionFor(Channel channel) {
		return clients.inverse().get(channel);
	}

	public void removeClient(Long session) {
		clients.remove(session);
	}

}
