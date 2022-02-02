using GameServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RavenfallServer.Providers
{
    public class NpcStateProvider : EntityStateProvider, INpcStateProvider
    {
        private ConcurrentDictionary<int, Dictionary<int, NpcAlignment>> alignments =
            new ConcurrentDictionary<int, Dictionary<int, NpcAlignment>>();

        private const string IdentifierDel = "_";
        private const string AttackTypeKey = "AttackType";
        private const string AttackTimeStatePrefix = "AttackTime";
        private const string InCombatState = "InCombat";
        private const string TagNPC = IdentifierDel + "Npc" + IdentifierDel;
        private const string TagPlayer = IdentifierDel + "Player" + IdentifierDel;


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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EnterCombat(NpcInstance npc, Player opponent)
        {
            return SetState(npc, InCombatState + TagPlayer + opponent.Id, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EnterCombat(NpcInstance npc, NpcInstance opponent)
        {
            return SetState(npc, InCombatState + TagNPC + opponent.Id, true);
        }

        public void ExitCombat(NpcInstance npc)
        {
            foreach (var key in State.Keys.Where(x => x.IndexOf(InCombatState + IdentifierDel, 0, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                State.TryRemove(key, out _);
            }
        }

        public bool InCombat(NpcInstance npc, Player opponent)
        {
            return GetState<bool>(npc, InCombatState + TagPlayer + opponent.Id);
        }
    }
}