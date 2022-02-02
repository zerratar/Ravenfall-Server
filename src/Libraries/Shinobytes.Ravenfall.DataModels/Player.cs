using Shinobytes.Ravenfall.Data.Entities;
using System;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Player : Entity<Player>
    {
        private int _UserId;
        private string _Name;
        private int _AppearanceId;
        private long _Coins;
        private int _TransformId;
        private DateTime _Created;
        private int skillsId;
        private int attributesId;
        private int health;
        private int endurance;
        private int level;
        private decimal experience;

        public int UserId { get => _UserId; set => Set(ref _UserId, value); }
        public int TransformId { get => _TransformId; set => Set(ref _TransformId, value); }
        public string Name { get => _Name; set => Set(ref _Name, value); }
        public int AppearanceId { get => _AppearanceId; set => Set(ref _AppearanceId, value); }
        public int ProfessionsId { get => skillsId; set => Set(ref skillsId, value); }
        public int AttributesId { get => attributesId; set => Set(ref attributesId, value); }
        public int Level { get => level; set => Set(ref level, value); }
        public decimal Experience { get => experience; set => Set(ref experience, value); }
        public int Health { get => health; set => Set(ref health, value); }
        public int Endurance { get => endurance; set => Set(ref endurance, value); }
        public long Coins { get => _Coins; set => Set(ref _Coins, value); }
        public DateTime Created { get => _Created; set => Set(ref _Created, value); }
    }
}