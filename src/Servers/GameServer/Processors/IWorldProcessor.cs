using System;
using GameServer.Managers;
using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.Processors
{
    public delegate bool EntityTickHandler<TObject>(Player player, TObject obj, TimeSpan totalTime, TimeSpan deltaTime);

    public interface IWorldProcessor
    {
        void RemovePlayer(Player player);
        void AddPlayer(Player player);
        IGameSession LinkToGameSession(string sessionKey, PlayerConnection connection);
        void SetTarget(Player player, GameObjectInstance obj);
        void SetTarget(Player player, NpcInstance npc);
        void SetTarget(NpcInstance npc, Player player);
        void SetCombatState(Player player, bool state);
        void SetCombatState(NpcInstance npc, bool state);
        void AttackTarget(Player player, int attackType);
        void PlayerObjectInteraction(Player player, GameObjectInstance serverObject, EntityActionInvoker action, int parameterId);
        void PlayAnimation(Player player, string animation, bool enabled = true, bool trigger = false, int number = 0);
        void PlayAnimation(NpcInstance npc, string animation, bool enabled = true, bool trigger = false, int number = 0);
        void SetItemEquipState(Player player, Item item, bool v);
        void SendChatMessage(Player player, int channelID, string message);
        void UpdatePlayerStat(Player player, string skill, int level, decimal exp);
        void PlayerStatLevelUp(Player player, string skill, int levelsGaiend);
        void AddPlayerItem(Player player, Item item, int amount = 1);
        void RemovePlayerItem(Player player, Item item, int amount = 1);
        void OpenTradeWindow(Player player, NpcInstance npc, string shopName, ShopInventory shopInventory);
        void PlayerNpcInteraction(Player player, NpcInstance npc, EntityActionInvoker action, int parameterId);
        void PlayerBuyItem(Player player, NpcInstance npc, int itemId, int amount);
        void PlayerSellItem(Player player, NpcInstance npc, int itemId, int amount);
        void DamageNpc(Player player, NpcInstance npc, int damage, int health, int maxHealth);
        void KillNpc(Player player, NpcInstance npc);
        void RespawnNpc(Player player, NpcInstance npc);
        void UpdateObject(GameObjectInstance obj);
        void SetEntityInterval<TObject>(
            Player player,
            TObject tree,
            EntityTickHandler<TObject> handleObjectTick)
            where TObject : IEntity;

        ITimeoutHandle SetEntityTimeout<TObject>(
            int milliseconds,
            Player player,
            TObject obj,
            EntityTickHandler<TObject> handleObjectTick)
            where TObject : IEntity;

        void ClearEntityTimeout(ITimeoutHandle handle);
    }
}