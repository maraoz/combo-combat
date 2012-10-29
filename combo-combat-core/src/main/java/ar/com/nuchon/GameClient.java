package ar.com.nuchon;

import java.net.InetSocketAddress;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import org.jboss.netty.bootstrap.ClientBootstrap;
import org.jboss.netty.channel.ChannelFuture;
import org.jboss.netty.channel.socket.nio.NioClientSocketChannelFactory;

import ar.com.nuchon.backend.tasks.GraphicsDisplayTask;
import ar.com.nuchon.backend.tasks.LineReaderTask;
import ar.com.nuchon.network.GameClientPipelineFactory;
import ar.com.nuchon.network.dispatch.MessageHubConfigurer;

import com.badlogic.gdx.backends.lwjgl.LwjglApplication;
import com.badlogic.gdx.backends.lwjgl.LwjglApplicationConfiguration;

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
		pool.execute(new LineReaderTask());
		
		LwjglApplicationConfiguration cfg = new LwjglApplicationConfiguration();
		cfg.title = "combo-combat";
		cfg.useGL20 = true;
		cfg.width = 800;
		cfg.height = 600;
		
		new LwjglApplication(new GraphicsDisplayTask(), cfg);
		

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
