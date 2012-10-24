package ar.com.game.handler.gui;

import ar.com.game.handler.base.BaseClientHandler;
import ar.com.game.network.dispatch.MessageListener;
import ar.com.game.network.message.gui.CloseNotify;
import ar.com.game.network.message.session.DisconnectRequest;

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
