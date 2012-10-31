package ar.com.nuchon.network;

import org.jboss.netty.channel.ChannelHandlerContext;
import org.jboss.netty.handler.timeout.IdleStateEvent;



public class GameServerNetworkHandler extends DispatcherNetworkHandler {
	
	@Override
	public void channelIdle(ChannelHandlerContext ctx, IdleStateEvent e)
			throws Exception {
		// TODO implementar que hacemos cuando un cliente no responde
	}

}
