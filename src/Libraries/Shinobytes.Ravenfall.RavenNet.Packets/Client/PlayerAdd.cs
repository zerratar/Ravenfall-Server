using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerAdd
    {
        public const short OpCode = 2;
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int Endurance { get; set; }
        public Attributes Attributes { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
        public Appearance Appearance { get; set; }

        public static PlayerAdd Create(IGameData gameData, Player player)
        {
            var appearance = gameData.GetAppearance(player.AppearanceId);
            var transform = gameData.GetTransform(player.TransformId);
            var attributes = gameData.GetAttributes(player.AttributesId);

            return new PlayerAdd
            {
                PlayerId = player.Id,
                Name = player.Name,
                Endurance = player.Endurance,
                Attributes = attributes,
                Position = transform.GetPosition(),
                Destination = transform.GetDestination(),
                Appearance = appearance,
                Health = player.Health,
                Level = player.Level
            };
        }
    }
}
