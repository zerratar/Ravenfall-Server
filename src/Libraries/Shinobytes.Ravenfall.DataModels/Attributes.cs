using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Attributes : Entity<Attributes>
    {
        private decimal _HealthExp;
        private decimal _DexterityExp;
        private decimal _IntellectExp;
        private decimal _EvasionExp;
        private decimal _EnduranceExp;
        private decimal _DefenseExp;
        private decimal _StrengthExp;
        private decimal _AttackExp;
        private decimal _AgilityExp;
        private int _Intellect;
        private int _Agility;
        private int _Dexterity;
        private int _Evasion;
        private int _Endurance;
        private int _Defense;
        private int _Strength;
        private int _Attack;
        private int _Health;

        public int Health { get => _Health; set => Set(ref _Health, value); }
        public int Attack { get => _Attack; set => Set(ref _Attack, value); }
        public int Strength { get => _Strength; set => Set(ref _Strength, value); }
        public int Defense { get => _Defense; set => Set(ref _Defense, value); }
        public int Dexterity { get => _Dexterity; set => Set(ref _Dexterity, value); }
        public int Agility { get => _Agility; set => Set(ref _Agility, value); }
        public int Intellect { get => _Intellect; set => Set(ref _Intellect, value); }
        public int Evasion { get => _Evasion; set => Set(ref _Evasion, value); }
        public int Endurance { get => _Endurance; set => Set(ref _Endurance, value); }
        public decimal EvasionExp { get => _EvasionExp; set => Set(ref _EvasionExp, value); }
        public decimal IntellectExp { get => _IntellectExp; set => Set(ref _IntellectExp, value); }
        public decimal AgilityExp { get => _AgilityExp; set => Set(ref _AgilityExp, value); }
        public decimal DexterityExp { get => _DexterityExp; set => Set(ref _DexterityExp, value); }
        public decimal DefenseExp { get => _DefenseExp; set => Set(ref _DefenseExp, value); }
        public decimal StrengthExp { get => _StrengthExp; set => Set(ref _StrengthExp, value); }
        public decimal HealthExp { get => _HealthExp; set => Set(ref _HealthExp, value); }
        public decimal AttackExp { get => _AttackExp; set => Set(ref _AttackExp, value); }
        public decimal EnduranceExp { get => _EnduranceExp; set => Set(ref _EnduranceExp, value); }        
    }
}