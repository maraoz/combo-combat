package ar.com.nuchon.network.message.event;

import ar.com.nuchon.network.BaseMessage;

public class GameStateAck extends BaseMessage {

	private final long sequence;

	public GameStateAck(long sequence) {
		super();
		this.sequence = sequence;
	}

	public long getSequence() {
		return sequence;
	}
	
}
