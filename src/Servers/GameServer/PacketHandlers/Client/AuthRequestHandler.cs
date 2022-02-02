using GameServer.Managers;
using GameServer.Network;
using GameServer.Services;
using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Linq;
using RavenNest.BusinessLogic.Data;
using static Shinobytes.Ravenfall.RavenNet.Packets.Client.UserPlayerList;

namespace GameServer.PacketHandlers
{
    public class AuthRequestHandler : PlayerPacketHandler<AuthRequest>
    {
        private readonly ILogger logger;
        private readonly IGameData gameData;
        private readonly IPlayerProvider playerProvider;
        private readonly IUserManager userManager;
        private readonly IAuthService authService;
        private readonly IPlayerConnectionProvider connectionProvider;
        private readonly IGameSessionManager sessionManager;

        public AuthRequestHandler(
            ILogger logger,
            IGameData gameData,
            IPlayerProvider playerProvider,
            IUserManager userManager,
            IAuthService authService,
            IPlayerConnectionProvider connectionProvider,
            IGameSessionManager sessionManager)
        {
            this.logger = logger;
            this.gameData = gameData;
            this.playerProvider = playerProvider;
            this.userManager = userManager;
            this.authService = authService;
            this.connectionProvider = connectionProvider;
            this.sessionManager = sessionManager;
        }

        protected override void Handle(AuthRequest data, PlayerConnection connection)
        {
#if DEBUG
            logger.LogDebug("Auth Request received. User: " + data.Username + ", Pass: " + data.Password + ", ClientVersion: " + data.ClientVersion);
#endif

            var user = userManager.Get(data.Username);
            if (user == null)
            {
#warning a user should not be created here.
                user = userManager.Create(data.Username, null, null, data.Password);
            }

            var result = authService.Authenticate(user, data.Password);

#if DEBUG
            logger.LogDebug("Sending Auth Response: @cya@" + (int)result);
#endif

            if (result != AuthResult.Success)
            {
                connection.Send(new AuthResponse() { Status = (int)result, SessionKeys = new byte[0] }, SendOption.Reliable);
                return;
            }

            // check if we already have a connection with the same user
            // and kick that user out if so by disconnecting that connection.
            var activeConnection = connectionProvider.GetConnection<PlayerConnection>(x => x.User?.Id == user.Id);
            if (activeConnection != null)
            {
                connectionProvider.Terminate(activeConnection, ConnectionKillSwitch.MultipleLocations);
            }

            connection.UserTag = user;

            // authenticated
            // send auth response
            connection.Send(new AuthResponse()
            {
                Status = 0,
                SessionKeys = user.Username.ToCharArray().Select(x => (byte)x).ToArray()
            }, SendOption.Reliable);

            // then send player list
            SendPlayerList(connection);

            // when player has been selected do the following
            //connection.PlayerTag = playerProvider.Get(data.Username);
            //worldProcessor.AddPlayer(connection);
            // SEE: UserPlayerSelectHandler, logic moved there.
        }

        private void SendPlayerList(PlayerConnection connection)
        {
            var players = playerProvider.GetPlayers(connection.User).ToArray();
            var appearances = players.Select(x => gameData.GetAppearance(x.AppearanceId)).ToArray();
            var sessions = players.Select(x => sessionManager.Get(x)).ToArray();

            connection.Send(UserPlayerList.Create(
                sessions.Select(x => new SessionInfo
                {
                    Id = x?.Id ?? -1,
                    Name = x?.Name
                }).ToArray(),
                players,
                appearances),
                SendOption.Reliable);
        }
    }
}
