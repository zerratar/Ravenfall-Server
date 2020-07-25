using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerLevelUp
    {
        public const short OpCode = 15;
        public int PlayerId { get; set; }
        public string Skill { get; set; }
        public int GainedLevels { get; set; }

        public static PlayerLevelUp Create(Player player, string skill, int gainedLevels)
        {
            return new PlayerLevelUp
            {
                PlayerId = player.Id,
                Skill = skill,
                GainedLevels = gainedLevels
            };
        }
    }
}
