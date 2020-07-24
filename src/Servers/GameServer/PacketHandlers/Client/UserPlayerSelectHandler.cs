using GameServer.Managers;
using GameServer.Processors;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class UserPlayerSelectHandler : PlayerPacketHandler<UserPlayerSelect>
    {
        private readonly IWorldProcessor worldProcessor;
        private readonly IPlayerProvider playerProvider;
        private readonly IGameSessionManager sessionManager;

        public UserPlayerSelectHandler(
            IWorldProcessor worldProcessor,
            IPlayerProvider playerProvider,
            IGameSessionManager sessionManager)
        {
            this.worldProcessor = worldProcessor;
            this.playerProvider = playerProvider;
            this.sessionManager = sessionManager;
        }

        protected override void Handle(UserPlayerSelect data, PlayerConnection connection)
        {
            var player = playerProvider.Get(data.PlayerId);
            if (player == null)
            {
                return;
            }

            // the disconnected event is only interesting after a player has
            // been selected, since the server wont keep track on a logged in user
            // without a selected player.

            // if the player is already in a game session
            // remove it from that session.
            var activeSession = sessionManager.Get(player);
            var targetSession = sessionManager.Get(data.SessionKey);
            if (!string.IsNullOrEmpty(player.Session) && activeSession != null && activeSession != targetSession)
            {
                worldProcessor.RemovePlayer(player);
            }

            connection.Disconnected -= ClientDisconnected;
            connection.Disconnected += ClientDisconnected;
            connection.PlayerTag = player;
            connection.SessionKey = data.SessionKey;

            worldProcessor.LinkToGameSession(data.SessionKey, connection);
        }

        private void ClientDisconnected(object sender, System.EventArgs e)
        {
            var connection = sender as PlayerConnection;
            connection.Disconnected -= ClientDisconnected;

            if (connection.Player == null)
                return;

            if (playerProvider.Remove(connection.Player.Id))
                worldProcessor.RemovePlayer(connection.Player);
        }
    }
}
