package ar.com.game.network;

import org.jboss.netty.channel.ChannelHandlerContext;
import org.jboss.netty.channel.ChannelStateEvent;

import ar.com.game.network.message.session.ConnectRequest;

public class GameClientNetworkHandler extends DispatcherNetworkHandler {

	/**
	 * clients need to send an initial message to server when channel is connected
	 */
	@Override
	public void channelConnected(ChannelHandlerContext ctx, ChannelStateEvent e) {
		// Send the first message because this is a client-side handler.
		e.getChannel().write(new ConnectRequest());
	}
	
}
