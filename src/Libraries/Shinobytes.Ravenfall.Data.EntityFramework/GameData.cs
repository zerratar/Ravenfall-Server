using GameServer;
using Microsoft.Extensions.Logging;
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
namespace Shinobytes.Ravenfall.Data.EntityFramework.Legacy
{
    public class GameData : IGameData
    {
        private const int SaveInterval = 10000;
        private const int SaveMaxBatchSize = 100;

        private readonly IRavenfallDbContextProvider db;
        private readonly ILogger logger;
        private readonly IKernel kernel;
        private readonly IQueryBuilder queryBuilder;

        private readonly EntitySet<Appearance, int> appearances;
        private readonly EntitySet<Player, int> players;
        private readonly EntitySet<InventoryItem, int> inventoryItems;
        private readonly EntitySet<ShopItem, int> shopInventoryItems;
        private readonly EntitySet<Transform, int> transforms;
        private readonly EntitySet<Item, int> items;
        private readonly EntitySet<Npc, int> npcs;
        private readonly EntitySet<GameObject, int> objects;
        private readonly EntitySet<User, int> users;
        private readonly EntitySet<Session, int> sessions;
        private readonly EntitySet<GameObjectInstance, int> objectInstances;
        private readonly EntitySet<NpcInstance, int> npcInstances;
        private readonly EntitySet<ItemDrop, int> itemDrops;

        private readonly IEntitySet[] entitySets;

        private ITimeoutHandle scheduleHandler;
        public object SyncLock { get; } = new object();

        public GameData(IRavenfallDbContextProvider db, ILogger logger, IKernel kernel, IQueryBuilder queryBuilder)
        {
            this.db = db;
            this.logger = logger;
            this.kernel = kernel;
            this.queryBuilder = queryBuilder;

            var stopWatch = new Stopwatch();

            stopWatch.Start();
            logger.LogDebug($"Connecting to the db...");
            using (var ctx = this.db.Get())
            {
                logger.LogDebug($"\tLoading db entries...");

                appearances = new EntitySet<Appearance, int>(ctx.Appearance.ToList(), i => i.Id);

                players = new EntitySet<Player, int>(ctx.Player.ToList(), i => i.Id);
                players.RegisterLookupGroup(nameof(User), x => x.UserId);

                // we can still store the game events, but no need to load them on startup as the DB will quickly be filled.
                // and take a long time to load

                itemDrops = new EntitySet<ItemDrop, int>(ctx.ItemDrop.ToList(), i => i.Id);


                sessions = new EntitySet<Session, int>(ctx.Session.ToList(), i => i.Id);
                objectInstances = new EntitySet<GameObjectInstance, int>(ctx.GameObjectInstance.ToList(), i => i.Id);
                objectInstances.RegisterLookupGroup(nameof(GameObject), x => x.ObjectId);
                objectInstances.RegisterLookupGroup(nameof(Session), x => x.SessionId);

                npcInstances = new EntitySet<NpcInstance, int>(ctx.NpcInstance.ToList(), i => i.Id);
                npcInstances.RegisterLookupGroup(nameof(Npc), x => x.NpcId);
                npcInstances.RegisterLookupGroup(nameof(Session), x => x.SessionId);

                inventoryItems = new EntitySet<InventoryItem, int>(ctx.InventoryItem.ToList(), i => i.Id);
                inventoryItems.RegisterLookupGroup(nameof(Player), x => x.PlayerId);

                shopInventoryItems = new EntitySet<ShopItem, int>(ctx.ShopItem.ToList(), i => i.Id);
                shopInventoryItems.RegisterLookupGroup(nameof(NpcInstance), x => x.NpcInstanceId);

                transforms = new EntitySet<Transform, int>(ctx.Transform.ToList(), i => i.Id);

                items = new EntitySet<Item, int>(ctx.Item.ToList(), i => i.Id);

                objects = new EntitySet<GameObject, int>(ctx.GameObject.ToList(), i => i.Id);

                npcs = new EntitySet<Npc, int>(ctx.Npc.ToList(), i => i.Id);

                users = new EntitySet<User, int>(ctx.User.ToList(), i => i.Id);

                entitySets = new IEntitySet[]
                {
                    appearances,
                    players,
                    inventoryItems,
                    shopInventoryItems,
                    objects,
                    items,
                    users,
                    npcs,
                    npcInstances,
                    objectInstances,
                    sessions,
                    transforms,
                };
            }

            stopWatch.Stop();
            logger.LogDebug($"All database entries loaded in {stopWatch.Elapsed.TotalSeconds} seconds.");

            ScheduleNextSave();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(GameObjectInstance entity) => Update(() => objectInstances.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Session entity) => Update(() => sessions.Add(entity));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(NpcInstance entity) => Update(() => npcInstances.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Item entity) => Update(() => items.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Appearance entity) => Update(() => appearances.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(GameObject obj) => Update(() => objects.Add(obj));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Player entity) => Update(() => players.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(User entity) => Update(() => users.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(InventoryItem entity) => Update(() => inventoryItems.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(ShopItem entity) => Update(() => shopInventoryItems.Add(entity));

        // This is not code, it is a shrimp. Cant you see?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Player GetPlayer(Func<Player, bool> predicate) =>
            players.Entities.FirstOrDefault(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public InventoryItem GetPlayerItem(int id, Func<InventoryItem, bool> predicate) =>
            players.TryGet(id, out var player)
                ? inventoryItems[nameof(Player), player.Id].FirstOrDefault(predicate)
                : null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<InventoryItem> GetPlayerItems(int id, Func<InventoryItem, bool> predicate) =>
            players.TryGet(id, out var player)
                ? inventoryItems[nameof(Player), player.Id].Where(predicate).ToList()
                : null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public User FindUser(Func<User, bool> predicate) =>
            users.Entities.FirstOrDefault(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public User FindUser(string userIdOrUsername) =>
            users.Entities.FirstOrDefault(x =>
                    x.TwitchId == userIdOrUsername ||
                    x.YouTubeId == userIdOrUsername ||
                    x.Username.Equals(userIdOrUsername, StringComparison.OrdinalIgnoreCase));


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<Npc> GetAllNpcs() => npcs.Entities.ToList();


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<ItemDrop> GetAllObjectItemDrops() => itemDrops.Entities.Where(x => x.EntityType == EntityType.Object).ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<ItemDrop> GetAllNpcItemDrops() => itemDrops.Entities.Where(x => x.EntityType == EntityType.NPC).ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<InventoryItem> GetAllPlayerItems(int characterId) =>
            inventoryItems[nameof(Player), characterId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<InventoryItem> GetInventoryItems(int characterId) =>
            inventoryItems[nameof(Player), characterId].Where(x => !x.Equipped).ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Player GetPlayer(int playerId) =>
            players[playerId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Player GetPlayerByUserId(int userId) =>
            players[nameof(User), userId].FirstOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Player GetPlayerByStreamId(string twitchUserId)
        {
            var user = GetUser(twitchUserId);
            return user == null ? null : players[nameof(User), user.Id].FirstOrDefault();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<Player> GetPlayers(Func<Player, bool> predicate) => players.Entities.Where(predicate).ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<Player> GetPlayers(User user) => players[nameof(User), user.Id];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<Player> GetAllPlayers() => players.Entities.ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<User> GetAllUsers() => users.Entities.ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<GameObject> GetAllGameObjects() => objects.Entities.ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public InventoryItem GetInventoryItem(int characterId, int itemId) =>
            inventoryItems[nameof(Player), characterId].FirstOrDefault(x => !x.Equipped && x.ItemId == itemId);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<InventoryItem> GetEquippedItems(int characterId) =>
            inventoryItems[nameof(Player), characterId]
                    .Where(x => x.Equipped)
                    .ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<InventoryItem> GetInventoryItems(int characterId, int itemId) =>
            inventoryItems[nameof(Player), characterId]
                    .Where(x => !x.Equipped && x.ItemId == itemId)
                    .ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Appearance GetAppearance(int appearanceId) => appearances[appearanceId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Session GetSession(string sessionKey) => sessions.Entities.OrderByDescending(x => x.Created).FirstOrDefault(x => x.Name == sessionKey);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Item GetItem(int id) => items[id];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<Item> GetAllItems() => items.Entities.ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public User GetUser(int userId) => users[userId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public User GetUser(string twitchUserId) => users.Entities
                .FirstOrDefault(x =>
                    x.Username.Equals(twitchUserId, StringComparison.OrdinalIgnoreCase) ||
                    x.TwitchId.Equals(twitchUserId, StringComparison.OrdinalIgnoreCase) ||
                    x.YouTubeId.Equals(twitchUserId, StringComparison.OrdinalIgnoreCase));


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(Appearance appearance) => appearances.Remove(appearance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(User user) => users.Remove(user);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(Player character) => players.Remove(character);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(InventoryItem invItem) => inventoryItems.Remove(invItem);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveRange(IReadOnlyList<InventoryItem> items) => items.ForEach(Remove);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Transform entity) => transforms.Add(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Transform GetTransform(int transformId) => transforms[transformId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(Transform transform) => transforms.Remove(transform);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(ShopItem shopItem) => shopInventoryItems.Remove(shopItem);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(GameObject obj) => objects.Remove(obj);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<ShopItem> GetShopItems(int npcInstanceId) => shopInventoryItems[nameof(NpcInstance), npcInstanceId];

        public ShopItem GetShopItem(int npcInstanceId, int itemId)
        {
            return shopInventoryItems[nameof(NpcInstance), npcInstanceId].FirstOrDefault(x => x.ItemId == itemId);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Npc GetNpc(int npcId) => npcs[npcId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GameObject GetGameObject(int objectId) => objects[objectId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Session GetSession(int sessionId) => sessions[sessionId];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NpcInstance GetNpcInstance(int npcInstanceId) => npcInstances[npcInstanceId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<NpcInstance> GetAllNpcInstances(int sessionId) => npcInstances[nameof(Session), sessionId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<GameObjectInstance> GetAllGameObjectInstances(int sessionId) => objectInstances[nameof(Session), sessionId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(GameObjectInstance obj) => objectInstances.Remove(obj);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(NpcInstance npc) => npcInstances.Remove(npc);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(Session session) => sessions.Remove(session);

        public Session CreateSession()
        {
            var id = GetNextId(sessions.Entities);
            var session = new Session()
            {
                Id = id,
                Created = DateTime.UtcNow
            };
            Add(session);
            return session;
        }

        public NpcInstance CreateNpcInstance()
        {
            var id = GetNextId(npcInstances.Entities);
            var npc = new NpcInstance()
            {
                Id = id
            };
            Add(npc);
            return npc;
        }

        public GameObjectInstance CreateGameObjectInstance()
        {
            var id = GetNextId(objectInstances.Entities);
            var obj = new GameObjectInstance()
            {
                Id = id
            };
            Add(obj);
            return obj;
        }

        public InventoryItem CreateInventoryItem()
        {
            var id = GetNextId(inventoryItems.Entities);
            var item = new InventoryItem
            {
                Id = id
            };

            Add(item);
            return item;
        }

        public ShopItem CreateShopItem()
        {
            var id = GetNextId(shopInventoryItems.Entities);
            var item = new ShopItem
            {
                Id = id
            };

            Add(item);
            return item;
        }

        public Appearance CreateAppearance(Appearance source)
        {
            var id = GetNextId(appearances.Entities);
            var appearance = new Appearance
            {
                Id = id,
                Gender = source.Gender,
                SkinColor = source.SkinColor,
                HairColor = source.HairColor,
                BeardColor = source.BeardColor,
                StubbleColor = source.StubbleColor,
                WarPaintColor = source.WarPaintColor,
                EyeColor = source.EyeColor,
                Eyebrows = source.Eyebrows,
                Hair = source.Hair,
                FacialHair = source.FacialHair,
                Head = source.Head,
                HelmetVisible = source.HelmetVisible
            };

            Add(appearance);
            return appearance;
        }
        public Appearance GenerateRandomAppearance()
        {
            var id = GetNextId(appearances.Entities);
            var gender = Utility.Random<Gender>();
            var skinColor = Utility.Random("#d6b8ae");
            var hairColor = Utility.Random("#A8912A", "#27ae60", "#2980b9", "#8e44ad");
            var beardColor = Utility.Random("#A8912A", "#27ae60", "#2980b9", "#8e44ad");
            var appearance = new Appearance
            {
                Id = id,
                Gender = gender,
                SkinColor = skinColor,
                HairColor = hairColor,
                BeardColor = beardColor,
                StubbleColor = skinColor,
                WarPaintColor = hairColor,
                EyeColor = Utility.Random("#000000", "#c0392b", "#2c3e50"),
                Eyebrows = Utility.Random(0, gender == Gender.Male ? 10 : 7),
                Hair = Utility.Random(0, 38),
                FacialHair = gender == Gender.Male ? Utility.Random(0, 18) : -1,
                Head = Utility.Random(0, 23),
                HelmetVisible = true
            };

            Add(appearance);
            return appearance;
        }

        public GameObject CreateGameObject()
        {
            var id = GetNextId(objects.Entities);
            var obj = new GameObject
            {
                Id = id
            };
            Add(obj);
            return obj;
        }


        public Transform CreateTransform()
        {
            var id = GetNextId(transforms.Entities);
            var transform = new Transform
            {
                Id = id
            };
            Add(transform);
            return transform;
        }

        public Player CreatePlayer()
        {
            var id = GetNextId(players.Entities);
            var player = new Player
            {
                Id = id,
                Created = DateTime.UtcNow
            };

            Add(player);
            return player;
        }

        public User CreateUser()
        {
            var id = GetNextId(users.Entities);
            var user = new User()
            {
                Id = id,
                Created = DateTime.UtcNow
            };

            Add(user);
            return user;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetNextId<T>(ICollection<T> entities) where T : IEntity
        {
            return entities.Count == 0 ? 0 : entities.Max(x => x.Id) + 1;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ScheduleNextSave()
        {
            if (scheduleHandler != null) return;
            scheduleHandler = kernel.SetTimeout(SaveChanges, SaveInterval);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Update(Action update)
        {
            if (update == null) return;
            update.Invoke();
            ScheduleNextSave();
        }

        public void Flush()
        {
            SaveChanges();
        }

        private void SaveChanges()
        {
            kernel.ClearTimeout(scheduleHandler);
            scheduleHandler = null;
            try
            {
                lock (SyncLock)
                {
                    var queue = BuildSaveQueue();
                    if (queue.Count > 0)
                    {
                        logger.LogDebug("Saving all pending changes to the database.");
                    }

                    using (var con = db.GetConnection())
                    {
                        con.Open();
                        while (queue.TryPeek(out var saveData))
                        {
                            var query = queryBuilder.Build(saveData);
                            if (query == null) return;

                            var command = con.CreateCommand();
                            command.CommandText = query.Command;

                            var result = command.ExecuteNonQuery();
                            if (result == 0)
                            {
                                logger.LogError("Unable to save data! Abort Query failed");
                                return;
                            }

                            ClearChangeSetState(saveData);

                            queue.Dequeue();
                        }
                        con.Close();
                    }
                }
            }

            catch (Exception exc)
            {
                logger.LogInformation("ERROR SAVING DATA!! " + exc);
            }
            finally
            {
                ScheduleNextSave();
            }
        }

        private void ClearChangeSetState(EntityStoreItems items = null)
        {
            foreach (var set in entitySets)
            {
                if (items == null)
                    set.ClearChanges();
                else
                    set.Clear(items.Entities);
            }
        }

        private Queue<EntityStoreItems> BuildSaveQueue()
        {
            var queue = new Queue<EntityStoreItems>();
            var addedItems = JoinChangeSets(entitySets.Select(x => x.Added).ToArray());
            foreach (var batch in CreateBatches(EntityState.Added, addedItems, SaveMaxBatchSize))
            {
                queue.Enqueue(batch);
            }

            var updateItems = JoinChangeSets(entitySets.Select(x => x.Updated).ToArray());
            foreach (var batch in CreateBatches(EntityState.Modified, updateItems, SaveMaxBatchSize))
            {
                queue.Enqueue(batch);
            }

            var deletedItems = JoinChangeSets(entitySets.Select(x => x.Removed).ToArray());
            foreach (var batch in CreateBatches(EntityState.Deleted, deletedItems, SaveMaxBatchSize))
            {
                queue.Enqueue(batch);
            }

            return queue;
        }

        private ICollection<EntityStoreItems> CreateBatches(EntityState state, ICollection<EntityChangeSet> items, int batchSize)
        {
            if (items == null || items.Count == 0) return new List<EntityStoreItems>();
            var batches = (int)Math.Floor(items.Count / (float)batchSize) + 1;
            var batchList = new List<EntityStoreItems>(batches);
            for (var i = 0; i < batches; ++i)
            {
                batchList.Add(new EntityStoreItems(state, items.Skip(i * batchSize).Take(batchSize).Select(x => x.Entity).ToList()));
            }
            return batchList;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ICollection<EntityChangeSet> JoinChangeSets(params ICollection<EntityChangeSet>[] changesets) =>
            changesets.SelectMany(x => x).OrderBy(x => x.LastModified).ToList();
    }
}
