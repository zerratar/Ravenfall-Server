using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Professions : Entity<Professions>
    {
        private decimal _SlayerExp;
        private decimal _WoodcuttingExp;
        private decimal _SailingExp;
        private decimal _FarmingExp;
        private decimal _FishingExp;
        private decimal _CookingExp;
        private decimal _MiningExp;
        private decimal _CraftingExp;
        private int _Mining;
        private int _Slayer;
        private int _Sailing;
        private int _Cooking;
        private int _Farming;
        private int _Woodcutting;
        private int _Crafting;
        private int _Fishing;

        public int Fishing { get => _Fishing; set => Set(ref _Fishing, value); }
        public int Mining { get => _Mining; set => Set(ref _Mining, value); }
        public int Crafting { get => _Crafting; set => Set(ref _Crafting, value); }
        public int Cooking { get => _Cooking; set => Set(ref _Cooking, value); }
        public int Woodcutting { get => _Woodcutting; set => Set(ref _Woodcutting, value); }
        public int Farming { get => _Farming; set => Set(ref _Farming, value); }
        public int Sailing { get => _Sailing; set => Set(ref _Sailing, value); }
        public int Slayer { get => _Slayer; set => Set(ref _Slayer, value); }
        public decimal FishingExp { get => _FishingExp; set => Set(ref _FishingExp, value); }
        public decimal MiningExp { get => _MiningExp; set => Set(ref _MiningExp, value); }
        public decimal CraftingExp { get => _CraftingExp; set => Set(ref _CraftingExp, value); }
        public decimal CookingExp { get => _CookingExp; set => Set(ref _CookingExp, value); }
        public decimal WoodcuttingExp { get => _WoodcuttingExp; set => Set(ref _WoodcuttingExp, value); }
        public decimal FarmingExp { get => _FarmingExp; set => Set(ref _FarmingExp, value); }
        public decimal SailingExp { get => _SailingExp; set => Set(ref _SailingExp, value); }
        public decimal SlayerExp { get => _SlayerExp; set => Set(ref _SlayerExp, value); }
    }
}