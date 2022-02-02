using GameServer.Managers;
using GameServer.Processors;
using Microsoft.Extensions.Logging;
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class UserPlayerSelectHandler : PlayerPacketHandler<UserPlayerSelect>
    {
        private readonly ILogger logger;
        private readonly IWorldProcessor worldProcessor;
        private readonly IGameData gameData;
        private readonly IGameSessionManager sessionManager;

        public UserPlayerSelectHandler(
            ILogger logger,
            IWorldProcessor worldProcessor,
            IGameData gameData,
            IGameSessionManager sessionManager)
        {
            this.logger = logger;
            this.worldProcessor = worldProcessor;
            this.gameData = gameData;
            this.sessionManager = sessionManager;
        }

        protected override void Handle(UserPlayerSelect data, PlayerConnection connection)
        {
            var player = gameData.GetPlayer(data.PlayerId);
            if (player == null)
            {
                return;
            }

            // the disconnected event is only interesting after a player has
            // been selected, since the server wont keep track on a logged in user
            // without a selected player.

            // if the player is already in a game session
            // remove it from that session.

            //var user = gameData.GetUser(player.UserId);
            var user = connection.User;
            var activeSession = sessionManager.Get(player);
            var targetSession = sessionManager.Get(data.SessionKey);
            if (activeSession != null && activeSession != targetSession)
            {
                logger.LogDebug("@gre@" + user.Username + "@whi@ sessions with character: @gre@" + player.Name + " @whi@from @gre@" + targetSession.Name + " to @whi@" + targetSession.Name);
                worldProcessor.RemovePlayer(player);
            }

            connection.Disconnected -= ClientDisconnected;
            connection.Disconnected += ClientDisconnected;
            connection.PlayerTag = player;
            connection.SessionKey = data.SessionKey;

            var session = worldProcessor.LinkToGameSession(data.SessionKey, connection);
            if (session != null)
            {
                logger.LogDebug("@gre@" + user.Username + "@whi@ joined the session @mag@'" + session.Name + "' @whi@with character @gre@" + player.Name);
            }
        }

        private void ClientDisconnected(object sender, System.EventArgs e)
        {
            var connection = sender as PlayerConnection;
            connection.Disconnected -= ClientDisconnected;

            if (connection.Player == null)
                return;

            worldProcessor.RemovePlayer(connection.Player);
        }
    }
}
