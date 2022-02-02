using GameServer.Managers;
using GameServer.Processors;
using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Packets.Bot;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class BotPlayerMessageHandler : PlayerPacketHandler<BotPlayerMessage>
    {
        private readonly ILogger logger;
        private readonly IWorldProcessor worldProcessor;
        private readonly IPlayerProvider playerProvider;
        private readonly IUserManager userManager;
        private readonly IGameSessionManager sessionManager;

        public BotPlayerMessageHandler(
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

        protected override void Handle(BotPlayerMessage data, PlayerConnection connection)
        {
            logger.LogDebug($"Player message session: {connection.User.Username}. User: {data.Username}");

            var user = userManager.Get(data.Username);
            if (user == null)
            {
                logger.LogDebug($"Cannot send a chat message ingame as the user does not exist.");
                return;
            }

            var gameSession = sessionManager.Get(data.Session);
            if (gameSession == null)
                return;

            var player = gameSession.Players.Get(user);//playerProvider.Get(user, data.CharacterIndex);
            if (player == null)
                return;

            worldProcessor.SendChatMessage(player, 0, data.Message);
        }
    }
}
