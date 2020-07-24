using GameServer.Managers;
using GameServer.Processors;
using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Packets.Bot;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class BotPlayerLeaveHandler : PlayerPacketHandler<BotPlayerLeave>
    {
        private readonly ILogger logger;
        private readonly IWorldProcessor worldProcessor;
        private readonly IPlayerProvider playerProvider;
        private readonly IUserManager userManager;
        private readonly IGameSessionManager sessionManager;

        public BotPlayerLeaveHandler(
            ILogger logger,
            IWorldProcessor worldProcessor,
            IPlayerProvider playerProvider,
            IUserManager userManager,
            IGameSessionManager sessionManager)
        {
            this.logger = logger;
            this.worldProcessor = worldProcessor;
            this.playerProvider = playerProvider;
            this.userManager = userManager;
            this.sessionManager = sessionManager;
        }

        protected override void Handle(BotPlayerLeave data, PlayerConnection connection)
        {
            logger.LogDebug($"Player leave Request from {connection.Player.Id}. User: {data.Username}, Character: {data.CharacterIndex}");

            var user = userManager.Get(data.Username);
            if (user == null)
            {
                logger.LogDebug($"Cannot leave game as the user does not exist.");
                return;
            }

            var player = playerProvider.Get(user, data.CharacterIndex);
            if (player == null)
            {
                // this user doesnt have any players yet.
                // create one for them.
                logger.LogDebug($"Cannot leave game, character specified does not exist.");
                return;
            }

            var session = sessionManager.Get(data.Session);
            if (session != null)
            {
                if (!sessionManager.InSession(user, session))
                {
                    // we cannot join with another player as we are already in
                    // this session.

                    logger.LogDebug($"Cannot leave game, user is not playing on this session.");
                    return;
                }

                if (session.Host != null && session.Host.InstanceID == connection.InstanceID)
                {
                    logger.LogDebug($"The host cannot leave the game.");
                    return;
                }

                worldProcessor.RemovePlayer(player);
            }
        }
    }
}
