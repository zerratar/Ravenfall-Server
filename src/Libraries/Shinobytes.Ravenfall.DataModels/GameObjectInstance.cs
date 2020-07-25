using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class GameObjectInstance : Entity<GameObjectInstance>
    {
        private int _ObjectId;
        private int _Type;
        private int _SessionId;
        public int ObjectId { get => _ObjectId; set => Set(ref _ObjectId, value); }
        public int Type { get => _Type; set => Set(ref _Type, value); }
        public int SessionId { get => _SessionId; set => Set(ref _SessionId, value); }
    }
}
