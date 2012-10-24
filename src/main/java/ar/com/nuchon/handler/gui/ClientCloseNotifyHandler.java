package ar.com.nuchon.handler.gui;

import ar.com.nuchon.handler.base.BaseClientHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.gui.CloseNotify;
import ar.com.nuchon.network.message.session.DisconnectRequest;

public class ClientCloseNotifyHandler extends BaseClientHandler implements
		MessageListener<CloseNotify> {

	private static final ClientCloseNotifyHandler INSTANCE = new ClientCloseNotifyHandler();

	private ClientCloseNotifyHandler() {
	}

	public static ClientCloseNotifyHandler get() {
		return INSTANCE;
	}

	public void handle(CloseNotify message) {
		send(new DisconnectRequest());
	}

}
