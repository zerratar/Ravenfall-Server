using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class EntityActions : Entity<EntityActions>
    {
        private EntityType _EntityType;
        private int _EntityId;
        public int EntityId { get => _EntityId; set => Set(ref _EntityId, value); }
        public EntityType EntityType { get => _EntityType; set => Set(ref _EntityType, value); }
        public string[] ActionTypes { get; set; }
    }
}
