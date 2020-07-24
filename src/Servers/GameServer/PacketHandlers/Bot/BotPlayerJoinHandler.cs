using Shinobytes.Ravenfall.RavenNet.Packets.Bot;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class BotPlayerJoinHandler : PlayerPacketHandler<BotPlayerJoin>
    {
        protected override void Handle(BotPlayerJoin data, PlayerConnection connection)
        {
        }
    }
}
