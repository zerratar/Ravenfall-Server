using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Packets.Bot;
using ROBot.Ravenfall;

namespace ROBot.Core.GameServer.PacketHandlers
{
    public class BotPlayerRemoveHandler : INetworkPacketHandler<BotPlayerRemove>
    {
        private readonly IStreamBotApplication app;
        private readonly ILogger logger;

        public BotPlayerRemoveHandler(IStreamBotApplication app, ILogger logger)
        {
            this.app = app;
            this.logger = logger;
        }

        public void Handle(BotPlayerRemove data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.LogDebug("Server Requests remove player: " + data.Username);
            app.OnPlayerRemove(data);
        }
    }
}
