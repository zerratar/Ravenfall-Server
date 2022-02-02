using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerCombatStateUpdate
    {
        public const short OpCode = 48;
        public int PlayerId { get; set; }
        public bool InCombat { get; set; }

        public static PlayerCombatStateUpdate Create(Player player, bool state)
        {
            return new PlayerCombatStateUpdate
            {
                InCombat = state,
                PlayerId = player.Id
            };
        }
    }
}
