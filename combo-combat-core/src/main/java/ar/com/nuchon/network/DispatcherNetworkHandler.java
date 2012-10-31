package ar.com.nuchon.network;

import java.util.logging.Logger;

import org.jboss.netty.channel.ChannelEvent;
import org.jboss.netty.channel.ChannelHandlerContext;
import org.jboss.netty.channel.ChannelState;
import org.jboss.netty.channel.ChannelStateEvent;
import org.jboss.netty.channel.ExceptionEvent;
import org.jboss.netty.channel.MessageEvent;
import org.jboss.netty.handler.timeout.IdleStateAwareChannelUpstreamHandler;

import ar.com.nuchon.network.dispatch.MessageHub;

/**
 * Common behaviour for both client and server network handlers. 
 * Makes all messages received be dispatched by the MessageHub. 
 * 
 * Also logs connection channel state change events 
 * and closes connections when exceptions occur
 *
 */
public abstract class DispatcherNetworkHandler extends IdleStateAwareChannelUpstreamHandler {
	
	private static final Logger logger = Logger
			.getLogger(DispatcherNetworkHandler.class.getName());


	/**
	 * routes the message using MessageHub each time a message is received.
	 */
	@Override
	public void messageReceived(ChannelHandlerContext ctx, MessageEvent e) {
		BaseMessage message = (BaseMessage) e.getMessage();
		message.setChannel(e.getChannel());
		MessageHub.route(message);
	}

	@Override
	public void exceptionCaught(ChannelHandlerContext ctx, ExceptionEvent e) {
		System.out.println(e.getCause().getClass()+" caught");
	}
	

	
	
	
	
	
	
	// log connections and disconnections
	@Override
	public void handleUpstream(ChannelHandlerContext ctx, ChannelEvent e)
			throws Exception {
		if (e instanceof ChannelStateEvent
				&& ((ChannelStateEvent) e).getState() != ChannelState.INTEREST_OPS) {
			logger.info(e.toString());
		}
		super.handleUpstream(ctx, e);
	}
	
}
