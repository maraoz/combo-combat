package ar.com.game.network;

import java.io.Serializable;
import java.net.InetSocketAddress;
import java.net.SocketAddress;

import org.jboss.netty.channel.Channel;

public abstract class BaseMessage implements Serializable {

	private Channel channel;

	public void setChannel(Channel channel) {
		this.channel = channel;
	}

	public Channel getChannel() {
		return channel;
	}

	public InetSocketAddress getSender() {
		SocketAddress remoteAddress = getChannel().getRemoteAddress();
		return (InetSocketAddress) remoteAddress;
	}

}
