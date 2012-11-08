package ar.com.nuchon.network;

import org.jboss.netty.channel.ChannelPipeline;
import org.jboss.netty.channel.ChannelPipelineFactory;
import org.jboss.netty.channel.Channels;
import org.jboss.netty.handler.codec.compression.ZlibDecoder;
import org.jboss.netty.handler.codec.compression.ZlibEncoder;
import org.jboss.netty.handler.codec.compression.ZlibWrapper;
import org.jboss.netty.handler.codec.serialization.ClassResolvers;
import org.jboss.netty.handler.codec.serialization.ObjectDecoder;
import org.jboss.netty.handler.codec.serialization.ObjectEncoder;


public abstract class BasePipelineFactory implements ChannelPipelineFactory {

	public ChannelPipeline getPipeline() {
		ChannelPipeline pipeline = Channels.pipeline();
		
		// compression
        pipeline.addLast("deflater", new ZlibEncoder(ZlibWrapper.GZIP));
        pipeline.addLast("inflater", new ZlibDecoder(ZlibWrapper.GZIP));
        
        // object marshalling
		pipeline.addLast("encoder", new ObjectEncoder());
		pipeline.addLast("decoder", new ObjectDecoder(ClassResolvers.weakCachingConcurrentResolver(null)));
		
		// consider adding IdleStateHandler
		
		//logic
		pipeline.addLast("logic", getLogicHandler());
		
		return pipeline;
		
	}

	public abstract DispatcherNetworkHandler getLogicHandler();
	
}
