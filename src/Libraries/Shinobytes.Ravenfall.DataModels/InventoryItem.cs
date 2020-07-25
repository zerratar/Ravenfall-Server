using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class InventoryItem : Entity<InventoryItem>
    {
        private int _PlayerId;
        private int _ItemId;
        private int _Amount;
        private bool _Equipped;

        public int PlayerId { get => _PlayerId; set => Set(ref _PlayerId, value); }
        public int ItemId { get => _ItemId; set => Set(ref _ItemId, value); }
        public int Amount { get => _Amount; set => Set(ref _Amount, value); }        
        public bool Equipped { get => _Equipped; set => Set(ref _Equipped, value); }
    }
}