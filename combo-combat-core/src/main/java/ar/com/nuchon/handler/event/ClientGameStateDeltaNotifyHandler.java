package ar.com.nuchon.handler.event;

import ar.com.nuchon.backend.ClientBackend;
import ar.com.nuchon.backend.domain.base.GameState;
import ar.com.nuchon.handler.base.BaseClientHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.event.GameStateAck;
import ar.com.nuchon.network.message.event.GameStateDeltaNotify;

public class ClientGameStateDeltaNotifyHandler extends BaseClientHandler implements
		MessageListener<GameStateDeltaNotify> {

	private static final ClientGameStateDeltaNotifyHandler INSTANCE = new ClientGameStateDeltaNotifyHandler();

	private ClientGameStateDeltaNotifyHandler() {
	}

	public static ClientGameStateDeltaNotifyHandler get() {
		return INSTANCE;
	}

	public void handle(GameStateDeltaNotify message) {
		GameState lastKnownState = ClientBackend.getState();
		GameState current = lastKnownState.applyDelta(message.getDelta());
		ClientBackend.setState(current);
		send(new GameStateAck(message.getDelta().getSequence()));
	}

}
