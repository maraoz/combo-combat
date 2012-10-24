package ar.com.nuchon.handler.update;

import ar.com.nuchon.backend.ClientBackend;
import ar.com.nuchon.handler.base.BaseClientHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.update.UpdatableCreateNotify;

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
