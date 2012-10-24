package ar.com.game.network.dispatch;

import ar.com.game.handler.chat.ClientChatNotifyHandler;
import ar.com.game.handler.chat.ServerChatRequestHandler;
import ar.com.game.handler.event.ClientBulletHitEventHandler;
import ar.com.game.handler.event.ServerBulletHitEventHandler;
import ar.com.game.handler.gui.ClientCloseNotifyHandler;
import ar.com.game.handler.gui.ClientLineTypedNotifyHandler;
import ar.com.game.handler.gui.ClientMouseMovedNotifyHandler;
import ar.com.game.handler.gui.ClientMoveKeyNotifyHandler;
import ar.com.game.handler.gui.ClientShootKeyNotifyHandler;
import ar.com.game.handler.move.ClientPlayerMoveNotifyHandler;
import ar.com.game.handler.move.ServerPlayerMoveRequestHandler;
import ar.com.game.handler.session.ClientConnectNotifyHandler;
import ar.com.game.handler.session.ClientConnectResponseHandler;
import ar.com.game.handler.session.ClientDisconnectNotifyHandler;
import ar.com.game.handler.session.ClientDisconnectResponseHandler;
import ar.com.game.handler.session.ServerConnectRequestHandler;
import ar.com.game.handler.session.ServerDisconnectRequestHandler;
import ar.com.game.handler.shoot.ServerBulletShotRequestHandler;
import ar.com.game.handler.update.ClientUpdatableCreateNotifyHandler;
import ar.com.game.network.message.chat.ChatNotify;
import ar.com.game.network.message.chat.ChatRequest;
import ar.com.game.network.message.event.BulletHitEvent;
import ar.com.game.network.message.gui.CloseNotify;
import ar.com.game.network.message.gui.LineTypedNotify;
import ar.com.game.network.message.gui.MouseMovedNotify;
import ar.com.game.network.message.gui.MoveKeyNotify;
import ar.com.game.network.message.gui.ShootKeyNotify;
import ar.com.game.network.message.move.PlayerMoveNotify;
import ar.com.game.network.message.move.PlayerMoveRequest;
import ar.com.game.network.message.session.ConnectNotify;
import ar.com.game.network.message.session.ConnectRequest;
import ar.com.game.network.message.session.ConnectResponse;
import ar.com.game.network.message.session.DisconnectNotify;
import ar.com.game.network.message.session.DisconnectRequest;
import ar.com.game.network.message.session.DisconnectResponse;
import ar.com.game.network.message.shoot.BulletShotRequest;
import ar.com.game.network.message.update.UpdatableCreateNotify;

public class MessageHubConfigurer {

	public static void setupServer() {

		MessageHub.subscribe(ChatRequest.class, ServerChatRequestHandler.get());
		// no handler for ChatNotify
		MessageHub.subscribe(ConnectRequest.class, ServerConnectRequestHandler.get());
		// no handler for ConnectNotify
		// no handler for ConnectResponse
		// no handler for LineTypedNotify
		MessageHub.subscribe(DisconnectRequest.class, ServerDisconnectRequestHandler.get());
		// no handler for CloseNotify
		// no handler for MouseMovedNotify
		MessageHub.subscribe(PlayerMoveRequest.class, ServerPlayerMoveRequestHandler.get());
		// no handler for PlayerMoveNotify
		// no handler for MoveKeyNotify
		// no handler for ShootKeyNotify
		MessageHub.subscribe(BulletShotRequest.class, ServerBulletShotRequestHandler.get());
		// no handler for UpdatableCreateNotify
		MessageHub.subscribe(BulletHitEvent.class, ServerBulletHitEventHandler.get());
		
	}

	public static void setupClient() {

		// no handler for ChatRequest
		MessageHub.subscribe(ChatNotify.class, ClientChatNotifyHandler.get());
		// no handler for ConnectRequest
		MessageHub.subscribe(ConnectNotify.class, ClientConnectNotifyHandler.get());
		MessageHub.subscribe(ConnectResponse.class, ClientConnectResponseHandler.get());
		MessageHub.subscribe(LineTypedNotify.class, ClientLineTypedNotifyHandler.get());
		// no handler for DisconnectRequest
		MessageHub.subscribe(DisconnectNotify.class, ClientDisconnectNotifyHandler.get());
		MessageHub.subscribe(DisconnectResponse.class, ClientDisconnectResponseHandler.get());
		MessageHub.subscribe(CloseNotify.class,  ClientCloseNotifyHandler.get());
		MessageHub.subscribe(MouseMovedNotify.class,  ClientMouseMovedNotifyHandler.get());
		// no handler for PlayerMoveRequest
		MessageHub.subscribe(PlayerMoveNotify.class,  ClientPlayerMoveNotifyHandler.get());
		MessageHub.subscribe(MoveKeyNotify.class,  ClientMoveKeyNotifyHandler.get());
		MessageHub.subscribe(ShootKeyNotify.class,  ClientShootKeyNotifyHandler.get());
		// no handler for BulletShotRequest
		MessageHub.subscribe(UpdatableCreateNotify.class,  ClientUpdatableCreateNotifyHandler.get());
		MessageHub.subscribe(BulletHitEvent.class, ClientBulletHitEventHandler.get());
		
	}

}
