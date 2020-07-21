using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Udp;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public class PlayerConnection : UserConnection
    {
        public PlayerConnection(
            ILogger logger,
            UdpConnection connection,
            INetworkPacketController packetHandler)
            : base(logger, connection, packetHandler)
        {
        }

        public Player Player => PlayerTag as Player;

        public IStreamBot Bot => BotTag as IStreamBot;

        public bool IsBot => Bot != null;
        public bool IsPlayer => Player != null;
    }

}
