package ar.com.game.handler.base;

import org.jboss.netty.channel.Channel;

import ar.com.game.network.BaseMessage;

import com.google.common.base.Preconditions;

public class BaseClientHandler {

	private static Channel serverChannel;
	private static Long mySession;

	public void setChannel(Channel channel) {
		Preconditions.checkState(serverChannel == null);
		serverChannel = channel;
	}

	public void setSession(Long session) {
		Preconditions.checkState(mySession == null);
		mySession = session;
	}

	private boolean isConnected() {
		return serverChannel != null && mySession != null;
	}

	public void send(BaseMessage message) {
		Preconditions.checkState(isConnected());
		serverChannel.write(message);
	}
}
