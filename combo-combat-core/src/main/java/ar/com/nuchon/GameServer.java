package ar.com.nuchon;


import java.net.InetSocketAddress;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import org.jboss.netty.bootstrap.ServerBootstrap;
import org.jboss.netty.channel.socket.nio.NioServerSocketChannelFactory;

import ar.com.nuchon.annotation.GameObjectAnnotationProcessor;
import ar.com.nuchon.backend.tasks.UpdateGameStateTask;
import ar.com.nuchon.network.GameServerPipelineFactory;
import ar.com.nuchon.network.dispatch.MessageHubConfigurer;

public class GameServer implements Runnable {

    public static final int SERVER_DEFAULT_PORT = 9462;

	public static void main(String[] args) throws Exception {
        new GameServer().run();
    }

	public void run() {
		
		// bind message handlers
		MessageHubConfigurer.setupServer();
		
		// scan annotations and do reflection magic
		GameObjectAnnotationProcessor.processDomainClasses();
		
		// create threads for server
		ExecutorService pool = Executors.newFixedThreadPool(5);
		pool.execute(new UpdateGameStateTask());
		
		// Configure the server.
        ServerBootstrap bootstrap = new ServerBootstrap(
                new NioServerSocketChannelFactory(
                        Executors.newCachedThreadPool(),
                        Executors.newCachedThreadPool()));

        // Set up the pipeline factory.
        bootstrap.setPipelineFactory(new GameServerPipelineFactory());

        // Bind and start to accept incoming connections.
        bootstrap.bind(new InetSocketAddress(SERVER_DEFAULT_PORT));
	}
}
