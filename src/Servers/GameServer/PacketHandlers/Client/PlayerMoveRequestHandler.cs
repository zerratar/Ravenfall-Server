using GameServer.Managers;
using GameServer.Network;
using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Server;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using RavenNest.BusinessLogic.Data;

namespace GameServer.PacketHandlers
{
    public class PlayerMoveRequestHandler : PlayerPacketHandler<PlayerMoveRequest>
    {
        private readonly ILogger logger;
        private readonly IGameData gameData;
        private readonly IPlayerStateProvider playerState;
        private readonly IPlayerConnectionProvider connectionProvider;
        private readonly IGameSessionManager sessionManager;

        public PlayerMoveRequestHandler(
            ILogger logger,
            IGameData gameData,
            IPlayerStateProvider playerState,
            IPlayerConnectionProvider connectionProvider,
            IGameSessionManager sessionManager)
        {
            this.logger = logger;
            this.gameData = gameData;
            this.playerState = playerState;
            this.connectionProvider = connectionProvider;
            this.sessionManager = sessionManager;
        }

        protected override void Handle(PlayerMoveRequest data, PlayerConnection connection)
        {
            if (connection.Player == null)
            {
#if RELEASE
                logger.LogError($"Move request from {connection.InstanceID}. But has not selected a player.");
#endif
                return;
            }

            logger.LogDebug($"Move Request from {connection.Player.Id} from {data.Position} to {data.Destination}");

            var player = connection.Player;

            var transform = gameData.GetTransform(player.TransformId);

            transform.SetPosition(data.Position);
            transform.SetDestination(data.Destination);

            var session = sessionManager.Get(player);

            // player moves, release any locked objects the player may have.
            session.Objects.ReleaseLocks(player);

            // exit combat if we are in one. This will cancel any ongoing attacks.
            playerState.ExitCombat(player);

            foreach (var playerConnection in connectionProvider.GetAllActivePlayerConnections(session))
            {
                playerConnection.Send(new PlayerMoveResponse()
                {
                    PlayerId = player.Id,
                    Destination = data.Destination,
                    Position = data.Position,
                    Running = data.Running
                }, Shinobytes.Ravenfall.RavenNet.SendOption.Reliable);
            }
        }
    }
}
