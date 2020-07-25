using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class NpcHealthChange
    {
        public const short OpCode = 40;
        public int NpcServerId { get; set; }
        public int PlayerId { get; set; }
        public int Delta { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public static NpcHealthChange Create(NpcInstance npc, Player player, int damage, int health, int maxHealth)
        {
            return new NpcHealthChange
            {
                NpcServerId = npc.Id,
                PlayerId = player.Id,
                Delta = damage,
                Health = health,
                MaxHealth = maxHealth
            };
        }
    }
}
