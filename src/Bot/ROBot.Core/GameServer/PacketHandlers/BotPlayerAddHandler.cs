using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Packets.Bot;
using ROBot.Ravenfall;

namespace ROBot.Core.GameServer.PacketHandlers
{
    public class BotPlayerAddHandler : INetworkPacketHandler<BotPlayerAdd>
    {
        private readonly IStreamBotApplication app;
        private readonly ILogger logger;

        public BotPlayerAddHandler(IStreamBotApplication app, ILogger logger)
        {
            this.app = app;
            this.logger = logger;
        }

        public void Handle(BotPlayerAdd data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.LogDebug("Server Requests add player: " + data.Username);
            app.OnPlayerAdd(data);
        }
    }
}
