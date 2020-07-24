using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using Shinobytes.Ravenfall.RavenNet.Packets.Bot;

namespace ROBot.Ravenfall.GameServer.PacketHandlers
{
    public class BotStreamDisconnectHandler : INetworkPacketHandler<BotStreamDisconnect>
    {
        private readonly IStreamBotApplication app;
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public BotStreamDisconnectHandler(IStreamBotApplication app, ILogger logger, IModuleManager moduleManager)
        {
            this.app = app;
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(BotStreamDisconnect data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.LogDebug("Server Requests Disconnect from Streamer: " + data.StreamID);

            app.StreamDisconnect(data);
        }
    }
}
