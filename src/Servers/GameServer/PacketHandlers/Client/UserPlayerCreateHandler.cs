using GameServer.Managers;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Linq;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using RavenNest.BusinessLogic.Data;

namespace GameServer.PacketHandlers
{
    public class UserPlayerCreateHandler : PlayerPacketHandler<UserPlayerCreate>
    {
        private readonly IGameData gameData;
        private readonly IPlayerProvider playerProvider;

        public UserPlayerCreateHandler(
            IGameData gameData,
            IPlayerProvider playerProvider)
        {
            this.gameData = gameData;
            this.playerProvider = playerProvider;
        }
        protected override void Handle(UserPlayerCreate data, PlayerConnection connection)
        {
            playerProvider.Create(connection.User, data.Name, data.Appearance);
            SendPlayerList(connection);
        }

        private void SendPlayerList(PlayerConnection connection)
        {
            var players = playerProvider.GetPlayers(connection.User).ToArray();
            var appearances = players.Select(x => gameData.GetAppearance(x.AppearanceId)).ToArray();
            connection.Send(UserPlayerList.Create(gameData, players, appearances), SendOption.Reliable);
        }
    }
}
