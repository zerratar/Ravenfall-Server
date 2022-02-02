using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class NpcAttack
    {
        public const short OpCode = 47;
        public int NpcServerId { get; set; }
        public int AttackType { get; set; }

        public static NpcAttack Create(NpcInstance npc, int attackType)
        {
            return new NpcAttack
            {
                NpcServerId = npc.Id,
                AttackType = attackType,
            };
        }
    }
}
