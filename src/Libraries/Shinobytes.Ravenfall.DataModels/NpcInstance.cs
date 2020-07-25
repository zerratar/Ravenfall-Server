using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class NpcInstance : Entity<NpcInstance>
    {
        public int NpcId { get; set; }
        public int TransformId { get; set; }
        public NpcAlignment Alignment { get; set; }
        public int SessionId { get; set; }
    }
}
