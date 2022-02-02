using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerTargetUpdate
    {
        public const short OpCode = 45;
        public int PlayerId { get; set; }
        public int TargetPlayerId { get; set; }
        public int TargetNpcId { get; set; }
        public int TargetObjectId { get; set; }

        public static PlayerTargetUpdate Create(Player player, GameObjectInstance obj)
        {
            return new PlayerTargetUpdate
            {
                PlayerId = player.Id,
                TargetNpcId = -1,
                TargetObjectId = obj.Id,
                TargetPlayerId = -1,
            };
        }

        public static PlayerTargetUpdate Create(Player player, NpcInstance npc)
        {
            return new PlayerTargetUpdate
            {
                PlayerId = player.Id,
                TargetNpcId = npc.Id,
                TargetObjectId = -1,
                TargetPlayerId = -1,
            };
        }
    }

}
