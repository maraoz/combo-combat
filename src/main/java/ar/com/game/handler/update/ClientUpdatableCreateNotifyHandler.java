package ar.com.game.handler.update;

import ar.com.game.backend.ClientBackend;
import ar.com.game.handler.base.BaseClientHandler;
import ar.com.game.network.dispatch.MessageListener;
import ar.com.game.network.message.update.UpdatableCreateNotify;

public class ClientUpdatableCreateNotifyHandler extends BaseClientHandler implements
		MessageListener<UpdatableCreateNotify> {

	private static final ClientUpdatableCreateNotifyHandler INSTANCE = new ClientUpdatableCreateNotifyHandler();

	private ClientUpdatableCreateNotifyHandler() {
	}

	public static ClientUpdatableCreateNotifyHandler get() {
		return INSTANCE;
	}

	public void handle(UpdatableCreateNotify message) {
		ClientBackend.addUpdatable(message.getUpdatable());

	}

}
