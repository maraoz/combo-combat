package ar.com.nuchon.handler.event;

import java.util.List;
import java.util.Map;

import ar.com.nuchon.backend.ServerBackend;
import ar.com.nuchon.backend.domain.base.GameState;
import ar.com.nuchon.backend.domain.base.GameStateDelta;
import ar.com.nuchon.handler.base.BaseServerHandler;
import ar.com.nuchon.network.dispatch.MessageListener;
import ar.com.nuchon.network.message.event.GameStateDeltaNotify;
import ar.com.nuchon.network.message.event.GameStateNotify;

import com.google.common.base.Preconditions;
import com.google.common.collect.Lists;
import com.google.common.collect.Maps;

public class ServerGameStateNotifyHandler extends BaseServerHandler implements
		MessageListener<GameStateNotify> {

	private static final ServerGameStateNotifyHandler INSTANCE = new ServerGameStateNotifyHandler();
	
	private ServerGameStateNotifyHandler() {
		ServerBackend.getStateMemory().add(new GameState());
	}

	public static ServerGameStateNotifyHandler get() {
		return INSTANCE;
	}

	public void handle(GameStateNotify message) {
		GameState current = message.getState();
		long currentStateId = current.getSequence();
		long latestStateId = ServerBackend.getLatestStateId();
		Map<Long, Long> sessionToLastAckState = ServerBackend.getSessionToLastAckState();
		List<GameState> stateMemory = ServerBackend.getStateMemory();
		
		
		Preconditions.checkArgument(currentStateId >= latestStateId);
		GameStateDelta delta;
		for (Long session :  getAllSessions()) {
			Long lastAckStateId = sessionToLastAckState.get(session);
			if (lastAckStateId == null) {
				lastAckStateId = 0L;
				sessionToLastAckState.put(session, lastAckStateId);
			}
			if (lastAckStateId != currentStateId) {
				GameState lastAckState = null;
				for (GameState state: stateMemory) {
					if (lastAckStateId == state.getSequence()) {
						lastAckState = state;
						break;
					}
				}
				if (lastAckState != null) {
					delta = lastAckState.getDeltaTo(current);
				} else {
					delta = new GameState().getDeltaTo(current);
				}
				send(new GameStateDeltaNotify(delta), session);
			}
		}
		if (currentStateId != latestStateId) {
			latestStateId = currentStateId;
			stateMemory.add(current);
		}
	}

}
