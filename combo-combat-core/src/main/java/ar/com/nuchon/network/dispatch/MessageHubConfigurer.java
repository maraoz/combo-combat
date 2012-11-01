package ar.com.nuchon.network.dispatch;

import ar.com.nuchon.handler.chat.ClientChatNotifyHandler;
import ar.com.nuchon.handler.chat.ServerChatRequestHandler;
import ar.com.nuchon.handler.event.ClientBulletHitEventHandler;
import ar.com.nuchon.handler.event.ServerBulletHitEventHandler;
import ar.com.nuchon.handler.gui.ClientCloseNotifyHandler;
import ar.com.nuchon.handler.gui.ClientLineTypedNotifyHandler;
import ar.com.nuchon.handler.gui.ClientMouseMovedNotifyHandler;
import ar.com.nuchon.handler.gui.ClientMoveKeyNotifyHandler;
import ar.com.nuchon.handler.gui.ClientShootKeyNotifyHandler;
import ar.com.nuchon.handler.move.ClientPlayerMoveNotifyHandler;
import ar.com.nuchon.handler.move.ServerPlayerMoveRequestHandler;
import ar.com.nuchon.handler.session.ClientConnectNotifyHandler;
import ar.com.nuchon.handler.session.ClientConnectResponseHandler;
import ar.com.nuchon.handler.session.ClientDisconnectNotifyHandler;
import ar.com.nuchon.handler.session.ClientDisconnectResponseHandler;
import ar.com.nuchon.handler.session.ServerConnectRequestHandler;
import ar.com.nuchon.handler.session.ServerDisconnectRequestHandler;
import ar.com.nuchon.handler.spells.ServerBulletShotRequestHandler;
import ar.com.nuchon.handler.update.ClientUpdatableCreateNotifyHandler;
import ar.com.nuchon.network.message.chat.ChatNotify;
import ar.com.nuchon.network.message.chat.ChatRequest;
import ar.com.nuchon.network.message.event.BulletHitNotify;
import ar.com.nuchon.network.message.gui.CloseNotify;
import ar.com.nuchon.network.message.gui.LineTypedNotify;
import ar.com.nuchon.network.message.gui.MouseMovedNotify;
import ar.com.nuchon.network.message.gui.MoveKeyNotify;
import ar.com.nuchon.network.message.gui.ShootKeyNotify;
import ar.com.nuchon.network.message.move.PlayerMoveNotify;
import ar.com.nuchon.network.message.move.PlayerMoveRequest;
import ar.com.nuchon.network.message.session.ConnectNotify;
import ar.com.nuchon.network.message.session.ConnectRequest;
import ar.com.nuchon.network.message.session.ConnectResponse;
import ar.com.nuchon.network.message.session.DisconnectNotify;
import ar.com.nuchon.network.message.session.DisconnectRequest;
import ar.com.nuchon.network.message.session.DisconnectResponse;
import ar.com.nuchon.network.message.spells.FireballCastRequest;
import ar.com.nuchon.network.message.update.UpdatableCreateNotify;

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
		MessageHub.subscribe(FireballCastRequest.class, ServerBulletShotRequestHandler.get());
		// no handler for UpdatableCreateNotify
		MessageHub.subscribe(BulletHitNotify.class, ServerBulletHitEventHandler.get());
		
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
		MessageHub.subscribe(BulletHitNotify.class, ClientBulletHitEventHandler.get());
		
	}

}
