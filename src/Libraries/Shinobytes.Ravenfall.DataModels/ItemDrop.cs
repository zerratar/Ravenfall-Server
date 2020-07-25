using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class ItemDrop : Entity<ItemDrop>
    {
        private int _ItemId;
        private double _DropChance; 
        private int _EntityId;
        private EntityType _EntityType;

        public int EntityId { get => _EntityId; set => Set(ref _EntityId, value); }
        public EntityType EntityType { get => _EntityType; set => Set(ref _EntityType, value); }
        public int ItemId { get => _ItemId; set => Set(ref _ItemId, value); }
        public double DropChance { get => _DropChance; set => Set(ref _DropChance, value); }
    }

}
