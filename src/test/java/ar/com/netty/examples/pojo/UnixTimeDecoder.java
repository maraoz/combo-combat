package ar.com.netty.examples.pojo;

import org.jboss.netty.buffer.ChannelBuffer;
import org.jboss.netty.channel.Channel;
import org.jboss.netty.channel.ChannelHandlerContext;
import org.jboss.netty.handler.codec.replay.ReplayingDecoder;
import org.jboss.netty.handler.codec.replay.VoidEnum;

public class UnixTimeDecoder extends ReplayingDecoder<VoidEnum> {

	@Override
    protected Object decode(
            ChannelHandlerContext ctx, Channel channel,
            ChannelBuffer buffer, VoidEnum state) {
        return new UnixTime(buffer.readBytes(4).readInt());
    }

}