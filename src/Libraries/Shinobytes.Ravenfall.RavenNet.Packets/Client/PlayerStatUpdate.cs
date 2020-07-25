using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerStatUpdate
    {
        public const short OpCode = 14;
        public int PlayerId { get; set; }
        public string Skill { get; set; }
        public int Level { get; set; }
        public decimal Experience { get; set; }

        public static PlayerStatUpdate Create(Player player, string skill, int level, decimal exp)
        {
            return new PlayerStatUpdate
            {
                PlayerId = player.Id,
                Skill = skill,
                Experience = exp,
                Level = level
            };
        }
    }
}
