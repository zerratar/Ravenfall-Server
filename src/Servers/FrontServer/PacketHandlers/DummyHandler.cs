using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace Shinobytes.Ravenfall.FrontServer.PacketHandlers
{
    public class DummyHandler : INetworkPacketHandler<RavenNet.Packets.Client.Dummy>
    {
        private int callCount;
        public void Handle(RavenNet.Packets.Client.Dummy data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            var client = connection as ServerClientConnection;
            if (client == null) return;
            if (data == null) return;
            ++callCount;
        }
    }
}