using Shinobytes.Ravenfall.RavenNet.Packets.Bot;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class BotPlayerTrainHandler : PlayerPacketHandler<BotPlayerTrain>
    {
        protected override void Handle(BotPlayerTrain data, PlayerConnection connection)
        {
        }
    }
}
