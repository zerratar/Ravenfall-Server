using Shinobytes.Ravenfall.RavenNet.Packets.Bot;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class BotPlayerLeaveHandler : PlayerPacketHandler<BotPlayerLeave>
    {
        protected override void Handle(BotPlayerLeave data, PlayerConnection connection)
        {
        }
    }
}
