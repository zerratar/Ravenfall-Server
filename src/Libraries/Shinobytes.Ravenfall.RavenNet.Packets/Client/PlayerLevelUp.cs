using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerLevelUp
    {
        public const short OpCode = 15;
        public int PlayerId { get; set; }
        public int Skill { get; set; }
        public int GainedLevels { get; set; }

        public static PlayerLevelUp Create(Player player, EntityStat stat, int gainedLevels)
        {
            return new PlayerLevelUp
            {
                PlayerId = player.Id,
                Skill = stat.Index,
                GainedLevels = gainedLevels
            };
        }
    }
}
