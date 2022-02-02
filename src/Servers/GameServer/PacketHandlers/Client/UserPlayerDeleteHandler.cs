using GameServer.Managers;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Linq;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using RavenNest.BusinessLogic.Data;
using GameServer.Processors;
using static Shinobytes.Ravenfall.RavenNet.Packets.Client.UserPlayerList;
using Microsoft.Extensions.Logging;

namespace GameServer.PacketHandlers
{
    public class UserPlayerDeleteHandler : PlayerPacketHandler<UserPlayerDelete>
    {
        private readonly ILogger logger;
        private readonly IGameData gameData;
        private readonly IWorldProcessor worldProcessor;
        private readonly IGameSessionManager sessionManager;

        public UserPlayerDeleteHandler(
            ILogger logger,
            IGameData gameData,
            IWorldProcessor worldProcessor,
            IGameSessionManager sessionManager)
        {
            this.logger = logger;
            this.gameData = gameData;
            this.worldProcessor = worldProcessor;
            this.sessionManager = sessionManager;
        }

        protected override void Handle(UserPlayerDelete data, PlayerConnection connection)
        {
            var player = gameData.GetPlayer(data.PlayerId);
            if (player == null)
            {
                return;
            }

            worldProcessor.RemovePlayer(player);
            if (gameData.RemovePlayer(player))
            {
                logger.LogDebug(connection.User.Username + " deleted a character (id: " + data.PlayerId + ", name: " + player.Name + ")");
                SendPlayerList(connection);
            }
            else
            {
                logger.LogError(connection.User.Username + " failed to delete one of its characters (id: " + data.PlayerId + ", name: " + player.Name + ")");
            }
        }

        private void SendPlayerList(PlayerConnection connection)
        {
            var players = gameData.GetPlayers(connection.User).ToArray();
            var appearances = players.Select(x => gameData.GetAppearance(x.AppearanceId)).ToArray();
            var sessions = players.Select(x => sessionManager.Get(x)).ToArray();

            connection.Send(UserPlayerList.Create(
                sessions.Select(x => new SessionInfo { Id = x?.Id ?? -1, Name = x?.Name }).ToArray(),
                players,
                appearances),
                SendOption.Reliable);
        }
    }
}
