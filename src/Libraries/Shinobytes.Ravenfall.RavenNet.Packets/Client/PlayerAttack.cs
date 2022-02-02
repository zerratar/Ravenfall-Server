using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerAttack
    {
        public const short OpCode = 46;
        public int PlayerId { get; set; }
        public int AttackType { get; set; }

        public static PlayerAttack Create(Player player, int attackType)
        {
            return new PlayerAttack
            {
                PlayerId = player.Id,
                AttackType = attackType,
            };
        }
    }
}
