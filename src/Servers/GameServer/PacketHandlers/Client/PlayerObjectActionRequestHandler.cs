﻿using GameServer.Managers;
using GameServer.Processors;
using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Server;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace GameServer.PacketHandlers
{
    public class PlayerObjectActionRequestHandler : PlayerPacketHandler<PlayerObjectActionRequest>
    {
        private readonly ILogger logger;
        private readonly IWorldProcessor worldProcessor;
        private readonly IGameSessionManager sessionManager;

        public PlayerObjectActionRequestHandler(
            ILogger logger,
            IWorldProcessor worldProcessor,
            IGameSessionManager sessionManager)
        {
            this.logger = logger;
            this.worldProcessor = worldProcessor;
            this.sessionManager = sessionManager;
        }
        protected override void Handle(PlayerObjectActionRequest data, PlayerConnection connection)
        {
            // actionId 0 is examine and is client side
            if (data.ActionId == 0)
            {
                logger.LogDebug(connection.Player.Name + ":" + connection.Player.Id + " sent examine action, ignoring.");
                return;
            }

            logger.LogDebug(connection.Player.Name + ":" + connection.Player.Id + " interacting with object: " + data.ObjectServerId + " action " + data.ActionId + " parameter " + data.ParameterId);

            var session = sessionManager.Get(connection.Player);
            var serverObject = session.Objects.Get(data.ObjectServerId);
            if (serverObject == null) return;
            var action = session.Objects.GetAction(serverObject, data.ActionId);
            if (action == null) return;

            // if we are already interacting with this object
            // ignore it.
            if (session.Objects.HasAcquiredLock(serverObject, connection.Player))
            {
                logger.LogDebug(connection.Player.Name + ":" + connection.Player.Id + " is already interacting with object. Ignore");
                return;
            }

            worldProcessor.PlayerObjectInteraction(connection.Player, serverObject, action, data.ParameterId);
        }
    }
}
