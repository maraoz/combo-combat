package ar.com.nuchon.network;


public class GameServerPipelineFactory extends BasePipelineFactory {

	@Override
	public DispatcherNetworkHandler getLogicHandler() {
		return new GameServerNetworkHandler();
	}

}
