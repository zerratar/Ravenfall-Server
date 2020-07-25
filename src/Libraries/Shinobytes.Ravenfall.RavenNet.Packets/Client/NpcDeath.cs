using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class NpcDeath
    {
        public const short OpCode = 41;
        public int NpcServerId { get; set; }
        public int PlayerId { get; set; }
        public static NpcDeath Create(NpcInstance npc, Player player)
        {
            return new NpcDeath
            {
                NpcServerId = npc.Id,
                PlayerId = player.Id,
            };
        }
    }
}
