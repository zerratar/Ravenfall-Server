using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class GameObject : Entity<GameObject>
    {
        private int _Type;
        private int _TransformId;
        private decimal _Experience;
        private int _InteractItemType;
        private int _RespawnMilliseconds;
        private bool _Static;

        public int Type { get => _Type; set => Set(ref _Type, value); }
        public int TransformId { get => _TransformId; set => Set(ref _TransformId, value); }
        public decimal Experience { get => _Experience; set => Set(ref _Experience, value); }
        public int InteractItemType { get => _InteractItemType; set => Set(ref _InteractItemType, value); }
        public int RespawnMilliseconds { get => _RespawnMilliseconds; set => Set(ref _RespawnMilliseconds, value); }
        public bool Static { get => _Static; set => Set(ref _Static, value); }
    }

    public class ExtendedGameObject : GameObject
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
    }
}
