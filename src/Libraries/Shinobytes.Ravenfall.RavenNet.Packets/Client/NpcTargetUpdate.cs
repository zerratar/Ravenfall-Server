using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class NpcTargetUpdate
    {
        public const short OpCode = 44;
        public int NpcServerId { get; set; }
        public int TargetPlayerId { get; set; }
        public int TargetNpcId { get; set; }

        public static NpcTargetUpdate Create(NpcInstance npc, Player player)
        {
            return new NpcTargetUpdate
            {
                NpcServerId = npc.Id,
                TargetPlayerId = player.Id,
            };
        }
    }
}
