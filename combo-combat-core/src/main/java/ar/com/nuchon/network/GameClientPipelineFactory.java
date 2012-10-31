package ar.com.nuchon.network;



public class GameClientPipelineFactory extends GamePipelineFactory {

	public DispatcherNetworkHandler getLogicHandler() {
		return new GameClientNetworkHandler();
	}
	
}
