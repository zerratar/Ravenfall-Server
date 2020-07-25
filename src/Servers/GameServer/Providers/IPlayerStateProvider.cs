using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace RavenfallServer.Providers
{
    public interface IEntityStateProvider
    {
        T GetState<T>(IEntity entity, string key);
        void SetState<T>(IEntity entity, string key, T model);
        void RemoveState(IEntity entity, string key);
    }

    public interface IPlayerStateProvider : IEntityStateProvider
    {
        void UpdateAttackTime(Player player, NpcInstance npc);

        void ClearAttackTime(Player player, NpcInstance npc);

        DateTime GetAttackTime(Player player, NpcInstance opponent);

        void ExitCombat(Player player);

        void EnterCombat(Player player, NpcInstance opponent);
        void EnterCombat(Player player, Player opponent);

        bool InCombat(Player player, NpcInstance opponent);
        bool InCombat(Player player, Player opponent);

        void SetAttackType(Player player, int attackType);
        int GetAttackType(Player player);
    }
}