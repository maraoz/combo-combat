package ar.com.game.main;

import java.net.InetSocketAddress;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import org.jboss.netty.bootstrap.ClientBootstrap;
import org.jboss.netty.channel.ChannelFuture;
import org.jboss.netty.channel.socket.nio.NioClientSocketChannelFactory;

import ar.com.game.backend.tasks.GraphicsDisplayTask;
import ar.com.game.backend.tasks.LineReaderTask;
import ar.com.game.network.GameClientPipelineFactory;
import ar.com.game.network.dispatch.MessageHubConfigurer;

public class GameClient implements Runnable {

	public static void main(String[] args) {
		new GameClient().run();
	}

	public void run() {
		// TODO: configuration file or command line arguments
		String host = "localhost";
		int port = GameServer.SERVER_DEFAULT_PORT;

		// bind message handlers
		MessageHubConfigurer.setupClient();

		// create GUI for client
		ExecutorService pool = Executors.newFixedThreadPool(5);
		pool.execute(new GraphicsDisplayTask());
		pool.execute(new LineReaderTask());
		

		// Configure the network client.
		NioClientSocketChannelFactory channelFactory = new NioClientSocketChannelFactory(
				Executors.newCachedThreadPool(),
				Executors.newCachedThreadPool());
		
		ClientBootstrap bootstrap = new ClientBootstrap(channelFactory);

		// Set up the game client pipeline factory.
		bootstrap.setPipelineFactory(new GameClientPipelineFactory());

		// Start the connection attempt.
		ChannelFuture future = bootstrap.connect(new InetSocketAddress(host, port));
		
		// wait for connection to end and release resources
		future.awaitUninterruptibly();
        if (!future.isSuccess()) {
            future.getCause().printStackTrace();
        }
        future.getChannel().getCloseFuture().awaitUninterruptibly();
        channelFactory.releaseExternalResources();
	}
}
