using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Client;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;

namespace Shinobytes.Ravenfall.TestClient
{
    public class TestClient : RavenClient
    {
        public int SentPacketCount;

        public TestClient(ILogger logger, IModuleManager moduleManager, INetworkPacketController controller)
            : base(logger, moduleManager, controller)
        {
        }

        public override void Send<T>(short packetId, T packet, SendOption sendOption)
        {
            ++SentPacketCount;
            base.Send(packetId, packet, sendOption);
        }

        public override void Send<T>(T packet, SendOption sendOption)
        {
            ++SentPacketCount;
            base.Send(packet, sendOption);
        }
    }
}
