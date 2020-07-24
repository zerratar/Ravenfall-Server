using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class NpcRespawn
    {
        public const short OpCode = 42;
        public int NpcServerId { get; set; }
        public int PlayerId { get; set; }
        public static NpcRespawn Create(Npc npc, Player player)
        {
            return new NpcRespawn
            {
                NpcServerId = npc.Id,
                PlayerId = player.Id,
            };
        }
    }
}
