using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class NpcInstance : Entity<NpcInstance>
    {
        private int health;
        private int endurance;

        public int NpcId { get; set; }
        public int TransformId { get; set; }
        public int Health { get => health; set => Set(ref health, value); }
        public int Endurance { get => endurance; set => Set(ref endurance, value); }
        public NpcAlignment Alignment { get; set; }
        public int SessionId { get; set; }
    }
}
