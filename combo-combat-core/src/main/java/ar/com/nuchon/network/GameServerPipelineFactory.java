package ar.com.nuchon.network;


public class GameServerPipelineFactory extends GamePipelineFactory {

	@Override
	public DispatcherNetworkHandler getLogicHandler() {
		return new GameServerNetworkHandler();
	}

}
