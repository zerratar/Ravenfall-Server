using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace ROBot.Ravenfall.GameServer.PacketHandlers
{
    public class BotStreamConnectHandler : INetworkPacketHandler<BotStreamConnect>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public BotStreamConnectHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(BotStreamConnect data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.LogDebug("Server Requests Connection to Streamer: " + data.StreamID);
        }
    }
}
