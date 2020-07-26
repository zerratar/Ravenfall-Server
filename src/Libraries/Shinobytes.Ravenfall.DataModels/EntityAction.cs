using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class EntityAction : Entity<EntityAction>
    {
        public int EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public string Action { get; set; }
    }
}