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
        private readonly IUserManager userProvider;
        private readonly IGameSessionManager sessionManager;

        public BotPlayerLeaveHandler(
            ILogger logger,
            IWorldProcessor worldProcessor,
            IPlayerProvider playerProvider,
            IUserManager userProvider,
            IGameSessionManager sessionManager)
        {
            this.logger = logger;
            this.worldProcessor = worldProcessor;
            this.playerProvider = playerProvider;
            this.userProvider = userProvider;
            this.sessionManager = sessionManager;
        }

        protected override void Handle(BotPlayerLeave data, PlayerConnection connection)
        {
        }
    }
}
