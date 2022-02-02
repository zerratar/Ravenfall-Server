using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace RavenfallServer.Providers
{
    public interface IEntityStateProvider
    {
        T GetState<T>(IEntity entity, string key);
        bool SetState<T>(IEntity entity, string key, T model);
        void RemoveState(IEntity entity, string key);
    }

    public interface IPlayerStateProvider : IEntityStateProvider
    {
        void UpdateAttackTime(Player player, NpcInstance npc);

        void ClearAttackTime(Player player, NpcInstance npc);

        DateTime GetAttackTime(Player player, NpcInstance opponent);

        void ExitCombat(Player player);

        bool EnterCombat(Player player, NpcInstance opponent);
        bool EnterCombat(Player player, Player opponent);

        bool InCombat(Player player, NpcInstance opponent);
        bool InCombat(Player player, Player opponent);

        void SetAttackType(Player player, int attackType);
        int GetAttackType(Player player);
    }
}