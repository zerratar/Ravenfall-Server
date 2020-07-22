using GameServer.Network;
using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Net;

namespace GameServer
{
    internal class Server : IRavenServer
    {
        const int gameServerPort = 8133;

        private readonly ILogger logger;
        private readonly RavenNetworkServer server;

        public Server(
            ILogger logger,
            IPlayerConnectionProvider connectionProvider)
        {
            this.logger = logger;
            server = new RavenNetworkServer(logger, connectionProvider);
        }

        public IRavenServer Start()
        {
            server.Start(IPAddress.Any, gameServerPort);
            logger.LogInformation("@whi@GameServer @mag@started @gray@on port @gre@" + gameServerPort);
            return this;
        }

        public void Dispose()
        {
            server.Dispose();
        }
    }
}
