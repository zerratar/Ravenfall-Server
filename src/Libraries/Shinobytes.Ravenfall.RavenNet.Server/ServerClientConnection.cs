using System;
using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Udp;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public class ServerClientConnection : RavenNetworkConnection
    {
        private readonly IServerRegistry registry;

        public ServerClientConnection(
            ILogger logger,
            UdpConnection connection,
            IServerRegistry registry,
            INetworkPacketController packetHandler)
            : base(logger, connection, packetHandler)
        {
            this.registry = registry;
            connection.Disconnected += Connection_Disconnected;
        }

        private void Connection_Disconnected(object sender, DisconnectedEventArgs e)
        {
            Logger.LogInformation($"[@whi@{this.Name ?? "???"}@gray@] Connection Closed.");
        }

        public string Name { get; private set; }
        public string ServerIp { get; private set; }
        public int ServerPort { get; private set; }

        public void OnServerDiscovery(string name, string serverIp, int serverPort)
        {
            registry.Register(name, this);

            this.Name = name;
            this.ServerIp = serverIp;
            this.ServerPort = serverPort;
            Logger.LogDebug($"[@whi@{this.Name ?? "???"}@gray@] Server Discovery. {serverIp}:{serverPort}");
        }
    }
}
