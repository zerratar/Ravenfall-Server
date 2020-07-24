using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets.Bot;
using ROBot.Ravenfall;

namespace ROBot.Core.GameServer.PacketHandlers
{
    public class BotStreamConnectHandler : INetworkPacketHandler<BotStreamConnect>
    {
        private readonly IStreamBotApplication app;
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public BotStreamConnectHandler(IStreamBotApplication app, ILogger logger, IModuleManager moduleManager)
        {
            this.app = app;
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(BotStreamConnect data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.LogDebug("Server Requests Connection to Streamer: " + data.TwitchId);

            app.BeginSession(data);
        }
    }
}
