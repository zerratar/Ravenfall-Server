using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class PlayerPositionUpdateHandler : PlayerPacketHandler<PlayerPositionUpdate>
    {
        private readonly IGameData gameData;

        public PlayerPositionUpdateHandler(IGameData gameData)
        {
            this.gameData = gameData;
        }
        protected override void Handle(PlayerPositionUpdate data, PlayerConnection connection)
        {
            var transform = gameData.GetTransform(connection.Player.TransformId);
            transform.SetPosition(data.Position);
        }
    }
}
