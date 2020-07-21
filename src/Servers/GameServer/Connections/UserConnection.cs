using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Udp;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public class UserConnection : RavenNetworkConnection
    {
        public UserConnection(
            ILogger logger,
            UdpConnection connection,
            INetworkPacketController packetHandler)
            : base(logger, connection, packetHandler)
        {
            Logger.Debug("Client[" + this.InstanceID + "] connected.");
            connection.Disconnected += Connection_Disconnected;
        }

        private void Connection_Disconnected(object sender, DisconnectedEventArgs e)
        {
            Logger.Debug("Client[" + this.InstanceID + "] disconnected.");
        }


        public User User => UserTag as User;
    }
}
