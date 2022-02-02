using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class NpcCombatStateUpdate
    {
        public const short OpCode = 49;
        public int NpcServerId { get; set; }
        public bool InCombat { get; set; }

        public static NpcCombatStateUpdate Create(NpcInstance npc, bool state)
        {
            return new NpcCombatStateUpdate
            {
                InCombat = state,
                NpcServerId = npc.Id
            };
        }
    }
}
