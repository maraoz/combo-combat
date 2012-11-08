package ar.com.nuchon.network;



public class GameClientPipelineFactory extends BasePipelineFactory {

	public DispatcherNetworkHandler getLogicHandler() {
		return new GameClientNetworkHandler();
	}
	
}
