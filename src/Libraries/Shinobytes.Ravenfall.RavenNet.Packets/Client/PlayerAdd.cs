using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerAdd
    {
        public const short OpCode = 2;
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int CombatLevel { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
        public Appearance Appearance { get; set; }
        public static PlayerAdd Create(Player player, Appearance appearance, Transform transform, int combatLevel)
        {
            return new PlayerAdd
            {
                PlayerId = player.Id,
                Name = player.Name,
                CombatLevel = combatLevel,
                Position = transform.GetPosition(),
#warning add health and maxhealth for player
                Destination = transform.GetDestination(),
                Appearance = appearance
            };
        }
    }
}
