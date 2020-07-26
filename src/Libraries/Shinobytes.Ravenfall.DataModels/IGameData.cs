using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Generic;

namespace RavenNest.BusinessLogic.Data
{
    public interface IGameData
    {
        void Add(Item entity);
        void Add(Session entity);
        void Add(Professions entity);
        void Add(Attributes entity);
        void Add(Appearance entity);
        void Add(Transform entity);
        void Add(Player entity);
        void Add(User entity);
        void Add(InventoryItem entity);
        void Add(ShopItem entity);
        void Add(GameObjectInstance entity);
        void Add(NpcInstance entity);

        Appearance GetAppearance(int appearanceId);
        Player GetPlayer(Func<Player, bool> predicate);
        InventoryItem GetPlayerItem(int id, Func<InventoryItem, bool> predicate);
        InventoryItem GetInventoryItem(int playerId, int itemId);
        User FindUser(Func<User, bool> predicate);
        ShopItem GetShopItem(int npcId, int id);
        User FindUser(string userIdOrUsername);
        Transform GetTransform(int transformId);
        Attributes GetAttributes(int attributesId);
        Professions GetProfessions(int professionsId);
        Player GetPlayer(int playerId);
        Player GetPlayerByUserId(int userId);
        Player GetPlayerByStreamId(string twitchUserId);
                
        EntityAction GetAction(int actionId);
        Item GetItem(int id);
        Npc GetNpc(int npcId);
        GameObject GetGameObject(int objectId);
        User GetUser(int userId);
        User GetUser(string twitchUserId);
        Session GetSession(int value);
        NpcInstance GetNpcInstance(int npcInstanceId);

        IReadOnlyList<EntityAction> GetActions(EntityType type);
        IReadOnlyList<EntityAction> GetActions(int entityId, EntityType type);
        IReadOnlyList<ShopItem> GetShopItems(int npcInstanceId);
        IReadOnlyList<Player> GetPlayers(User user);
        IReadOnlyList<Player> GetPlayers(Func<Player, bool> predicate);
        IReadOnlyList<InventoryItem> GetEquippedItems(int playerId);
        IReadOnlyList<InventoryItem> GetAllPlayerItems(int playerId);
        IReadOnlyList<InventoryItem> GetInventoryItems(int playerId);
        IReadOnlyList<InventoryItem> GetPlayerItems(int id, Func<InventoryItem, bool> predicate);
        IReadOnlyList<InventoryItem> GetInventoryItems(int playerId, int itemId);
        IReadOnlyList<NpcInstance> GetAllNpcInstances(int sessionId);
        IReadOnlyList<GameObjectInstance> GetAllGameObjectInstances(int sessionId);
        IReadOnlyList<User> GetAllUsers();
        IReadOnlyList<Player> GetAllPlayers();
        IReadOnlyList<Npc> GetAllNpcs();
        IReadOnlyList<Item> GetAllItems();
        IReadOnlyList<GameObject> GetAllGameObjects();
        IReadOnlyList<ItemDrop> GetAllObjectItemDrops();
        IReadOnlyList<ItemDrop> GetAllNpcItemDrops();


        void Remove(Professions entity);
        void Remove(Attributes entity);
        void Remove(GameObjectInstance obj);
        void Remove(NpcInstance npc);
        void Remove(Session session);
        void Remove(GameObject obj);
        void Remove(Appearance appearance);
        void Remove(Transform transform);
        void Remove(User user);
        void Remove(Player character);
        void Remove(InventoryItem invItem);
        void RemoveRange(IReadOnlyList<InventoryItem> items);
        void Remove(ShopItem shopItem);

        Attributes CreateAttributes();
        Professions CreateProfessions();
        GameObject CreateGameObject();
        Appearance CreateAppearance(Appearance source);
        User CreateUser();
        Session CreateSession();
        NpcInstance CreateNpcInstance();
        GameObjectInstance CreateGameObjectInstance();
        InventoryItem CreateInventoryItem();
        ShopItem CreateShopItem();
        Appearance GenerateRandomAppearance();
        Transform CreateTransform();
        Session GetSession(string sessionKey);

        Player CreatePlayer();

        void Flush();

        object SyncLock { get; }

    }
}
