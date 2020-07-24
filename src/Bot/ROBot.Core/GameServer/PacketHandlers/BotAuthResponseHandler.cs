using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets.Bot;

namespace ROBot.Core.GameServer.PacketHandlers
{
    public class BotAuthResponseHandler : INetworkPacketHandler<BotAuthResponse>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public BotAuthResponseHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(BotAuthResponse data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.LogDebug("Login response: " + data.Status);
            var auth = moduleManager.GetModule<BotAuthentication>();
            if (auth != null)
            {
                auth.SetResult(data.Status);
            }
        }
    }
}
