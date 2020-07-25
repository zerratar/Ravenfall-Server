using Shinobytes.Ravenfall.Data.Entities;
using System;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Player : Entity<Player>
    {
        private int _UserId;
        private string _Name;
        private int _AppearanceId;
        private int _StatsId;
        private long _Coins;
        private int? _Session;
        private int _TransformId;
        private DateTime _Created;

        public int UserId { get => _UserId; set => Set(ref _UserId, value); }
        public int TransformId { get => _TransformId; set => Set(ref _TransformId, value); }
        public string Name { get => _Name; set => Set(ref _Name, value); }
        public int AppearanceId { get => _AppearanceId; set => Set(ref _AppearanceId, value); }
        public int StatsId { get => _StatsId; set => Set(ref _StatsId, value); }
        public long Coins { get => _Coins; set => Set(ref _Coins, value); }
        public int? SessionId { get => _Session; set => Set(ref _Session, value); }
        public DateTime Created { get => _Created; set => Set(ref _Created, value); }
    }

    public class Stats : Entity<Stats>
    {
    }
}