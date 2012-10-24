package ar.com.game.network;

import org.jboss.netty.channel.ChannelPipeline;
import org.jboss.netty.channel.ChannelPipelineFactory;
import org.jboss.netty.channel.Channels;
import org.jboss.netty.handler.codec.compression.ZlibDecoder;
import org.jboss.netty.handler.codec.compression.ZlibEncoder;
import org.jboss.netty.handler.codec.compression.ZlibWrapper;
import org.jboss.netty.handler.codec.serialization.ObjectDecoder;
import org.jboss.netty.handler.codec.serialization.ObjectEncoder;

public class GameServerPipelineFactory implements ChannelPipelineFactory {

	public ChannelPipeline getPipeline() {
		ChannelPipeline pipeline = Channels.pipeline();
		
		// Enable stream compression (you can remove these two if unnecessary)
        pipeline.addLast("deflater", new ZlibEncoder(ZlibWrapper.GZIP));
        pipeline.addLast("inflater", new ZlibDecoder(ZlibWrapper.GZIP));
        
		pipeline.addLast("encoder", new ObjectEncoder());
		pipeline.addLast("decoder", new ObjectDecoder());
		pipeline.addLast("handler", new GameServerNetworkHandler());
		
		return pipeline;
	}

}
