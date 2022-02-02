using GameServer.Managers;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Linq;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using RavenNest.BusinessLogic.Data;
using static Shinobytes.Ravenfall.RavenNet.Packets.Client.UserPlayerList;
using Microsoft.Extensions.Logging;

namespace GameServer.PacketHandlers
{
    public class UserPlayerCreateHandler : PlayerPacketHandler<UserPlayerCreate>
    {
        private readonly ILogger logger;
        private readonly IGameData gameData;
        private readonly IPlayerProvider playerProvider;
        private readonly IGameSessionManager sessionManager;

        public UserPlayerCreateHandler(
            ILogger logger,
            IGameData gameData,
            IPlayerProvider playerProvider,
            IGameSessionManager sessionManager)
        {
            this.logger = logger;
            this.gameData = gameData;
            this.playerProvider = playerProvider;
            this.sessionManager = sessionManager;
        }
        protected override void Handle(UserPlayerCreate data, PlayerConnection connection)
        {
            try
            {
                var created = playerProvider.Create(connection.User, data.Name, data.Appearance);
                logger.LogDebug(connection.User.Username + " created a new character with name " + created.Name);
            }
            finally
            {
                SendPlayerList(connection);
            }
        }

        private void SendPlayerList(PlayerConnection connection)
        {
            var players = playerProvider.GetPlayers(connection.User).ToArray();
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
