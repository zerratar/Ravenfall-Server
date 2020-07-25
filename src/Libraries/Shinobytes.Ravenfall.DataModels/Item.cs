using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Item : Entity<Item>
    {
        // code structure is a bit ugly here but
        // it is because it was auto generated with:

        // REGEX REPLACE: public ([a-zA-Z0-9|\?]+) ([a-zA-Z0-9]+) { get; set; }
        //          WITH: private $1 _$2; public $1 $2 { get => _$2; set => Set(ref _$2, value); }
        // To save some time. uggah

        private string _Name;
        private bool _Stackable;
        private bool _Equippable;
        private bool _Consumable;
        private int _Tier;
        private int _Type;
        private int _Value;

        public string Name { get => _Name; set => Set(ref _Name, value); }
        public bool Stackable { get => _Stackable; set => Set(ref _Stackable, value); }
        public bool Equippable { get => _Equippable; set => Set(ref _Equippable, value); }
        public bool Consumable { get => _Consumable; set => Set(ref _Consumable, value); }
        public int Tier { get => _Tier; set => Set(ref _Tier, value); }
        public int Type { get => _Type; set => Set(ref _Type, value); }
        public int Value { get => _Value; set => Set(ref _Value, value); }
    }
}
