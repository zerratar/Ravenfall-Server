using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Npc : Entity<Npc>
    {
        public int NpcId { get; set; }
        public int TransformId { get; set; }
        public int AttributesId { get; set; }
        public int Level { get; set; }
        public int RespawnTimeMs { get; set; }
        public NpcAlignment Alignment { get; set; }
    }
}
