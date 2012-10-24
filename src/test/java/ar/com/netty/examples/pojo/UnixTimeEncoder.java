package ar.com.netty.examples.pojo;
import static org.jboss.netty.buffer.ChannelBuffers.buffer;

import org.jboss.netty.buffer.ChannelBuffer;
import org.jboss.netty.channel.ChannelHandlerContext;
import org.jboss.netty.channel.Channels;
import org.jboss.netty.channel.MessageEvent;
import org.jboss.netty.channel.SimpleChannelHandler;

public class UnixTimeEncoder extends SimpleChannelHandler {

    @Override
	public void writeRequested(ChannelHandlerContext ctx, MessageEvent e) {
        UnixTime time = (UnixTime) e.getMessage();
        
        ChannelBuffer buf = buffer(4);
        buf.writeInt(time.getValue());
        
        Channels.write(ctx, e.getFuture(), buf);
    }
}