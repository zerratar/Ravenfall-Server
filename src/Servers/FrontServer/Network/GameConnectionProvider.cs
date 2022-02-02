using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server;
using Shinobytes.Ravenfall.RavenNet.Udp;

namespace FrontServer.Network
{
    public class GameConnectionProvider : RavenConnectionProvider
    {
        public GameConnectionProvider(ILogger logger, INetworkPacketController packetHandlers)
            : base(logger, packetHandlers)
        {
        }

        protected override RavenNetworkConnection CreateConnection(ILogger logger, UdpConnection connection, INetworkPacketController packetHandlers)
        {
            return new RavenNetworkConnection(logger, connection, packetHandlers);
        }
    }
}
