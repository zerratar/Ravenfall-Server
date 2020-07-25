using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RavenfallServer.Providers
{
    public class NpcStateProvider : INpcStateProvider
    {
        private ConcurrentDictionary<int, Dictionary<int, NpcAlignment>> alignments =
            new ConcurrentDictionary<int, Dictionary<int, NpcAlignment>>();

        public NpcAlignment SetAlignment(Player player, NpcInstance npc, NpcAlignment alignment)
        {
            if (alignments.TryGetValue(player.Id, out var a))
            {
                a[npc.Id] = npc.Alignment;
                return npc.Alignment;
            }

            alignments[player.Id] = new Dictionary<int, NpcAlignment> {
                {
                    npc.Id,
                    npc.Alignment
                }
            };

            return alignments[player.Id][npc.Id];
        }

        public NpcAlignment GetAlignment(Player player, NpcInstance npc)
        {
            if (alignments.TryGetValue(player.Id, out var a))
            {
                if (a.TryGetValue(npc.Id, out var alignment))
                {
                    return alignment;
                }
            }

            return SetAlignment(player, npc, npc.Alignment);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEnemy(Player player, NpcInstance npc)
        {
            return GetAlignment(player, npc) == NpcAlignment.Enemy;
        }

        public void ExitCombat(NpcInstance npc)
        {
        }

        public void EnterCombat(NpcInstance npc, Player player)
        {
        }
    }
}