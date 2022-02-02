using GameServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RavenfallServer.Providers
{
    public class PlayerStateProvider : EntityStateProvider, IPlayerStateProvider
    {
        private const string IdentifierDel = "_";
        private const string AttackTypeKey = "AttackType";
        private const string AttackTimeStatePrefix = "AttackTime";
        private const string InCombatState = "InCombat";
        private const string TagNPC = IdentifierDel + "Npc" + IdentifierDel;
        private const string TagPlayer = IdentifierDel + "Player" + IdentifierDel;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAttackType(Player player, int attackType)
        {
            SetState(player, AttackTypeKey, attackType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetAttackType(Player player)
        {
            return GetState<int>(player, AttackTypeKey);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTime GetAttackTime(Player player, NpcInstance npc)
        {
            return GetState<DateTime>(player, AttackTimeStatePrefix + TagNPC + npc.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateAttackTime(Player player, NpcInstance npc)
        {
            SetState(player, AttackTimeStatePrefix + TagNPC + npc.Id, DateTime.UtcNow);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearAttackTime(Player player, NpcInstance npc)
        {
            SetState(player, AttackTimeStatePrefix + TagNPC + npc.Id, DateTime.MinValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EnterCombat(Player player, Player opponent)
        {
            return SetState(player, InCombatState + TagPlayer + opponent.Id, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EnterCombat(Player player, NpcInstance opponent)
        {
            return SetState(player, InCombatState + TagNPC + opponent.Id, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool InCombat(Player player, Player opponent)
        {
            return GetState<bool>(player, InCombatState + TagPlayer + opponent.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool InCombat(Player player, NpcInstance opponent)
        {
            return GetState<bool>(player, InCombatState + TagNPC + opponent.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ExitCombat(Player player)
        {
            foreach (var key in State.Keys.Where(x => x.IndexOf(InCombatState + IdentifierDel, 0, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                State.TryRemove(key, out _);
            }
        }

    }
}