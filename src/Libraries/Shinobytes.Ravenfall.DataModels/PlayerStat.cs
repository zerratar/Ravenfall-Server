using Shinobytes.Ravenfall.Core;
using System;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class EntityStat
    {
        public string Name { get; set; }
        public decimal Experience { get; set; }
        public int Level { get; set; }

        public int AddExperience(decimal amount)
        {
            var delta = GameMath.ExperienceToLevel(Experience) - this.Level;
            Experience += amount;
            Level += delta;
            return delta;
        }

        public static EntityStat Create(string name, int level = 1, decimal experience = 0)
        {
            var levelExp = GameMath.LevelToExperience(level);
            return new EntityStat
            {
                Name = name,
                Level = level,
                Experience = experience > 0 ? experience : levelExp
            };
        }
    }
}
