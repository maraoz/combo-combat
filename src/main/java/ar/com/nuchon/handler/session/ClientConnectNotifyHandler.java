package ar.com.nuchon.handler.session;

import ar.com.nuchon.handler.base.BaseClientHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.session.ConnectNotify;

public class ClientConnectNotifyHandler extends BaseClientHandler implements
		MessageListener<ConnectNotify> {

	private static final ClientConnectNotifyHandler INSTANCE = new ClientConnectNotifyHandler();

	private ClientConnectNotifyHandler() {
	}

	public static ClientConnectNotifyHandler get() {
		return INSTANCE;
	}

	public void handle(ConnectNotify message) {
		System.out.println(message.getWho() + " connected to chat.");

	}

}
