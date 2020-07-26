using GameServer.Repositories;
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Managers
{
    public class ObjectManager : EntityManager, IObjectManager
    {
        private readonly ConcurrentDictionary<int, ItemDrop[]> objectItemDrops
            = new ConcurrentDictionary<int, ItemDrop[]>();

        private readonly List<GameObjectInstance> entities = new List<GameObjectInstance>();
        private readonly object mutex = new object();
        private readonly IoC ioc;
        private readonly Session gameSession;
        private readonly IGameData gameData;
        private int index = 0;

        public ObjectManager(
            IoC ioc,
            Session gameSession,
            IGameData gameData)
            : base(ioc, gameData)

        {
            this.ioc = ioc;
            this.gameSession = gameSession;
            this.gameData = gameData;

            AddGameObjects();
            AddObjectDrops();
            AddActions(EntityType.Object);
        }

        public ItemDrop[] GetItemDrops(GameObjectInstance obj)
        {
            var type = gameData.GetGameObject(obj.ObjectId).Type;
            if (objectItemDrops.TryGetValue(type, out var drops))
            {
                return drops;
            }

            return new ItemDrop[0];
        }

        public GameObjectInstance Get(int instanceId)
        {
            lock (mutex)
            {
                return entities.FirstOrDefault(x => x.Id == instanceId);
            }
        }

        public EntityActionInvoker GetAction(GameObjectInstance obj, int actionId)
        {
            var type = gameData.GetGameObject(obj.ObjectId).Type;
            if (entityActions.TryGetValue(type, out var actions))
            {
                return actions.Select(x => x.Value).FirstOrDefault(x => x.Id == actionId);
            }
            return null;
        }

        public IReadOnlyList<GameObjectInstance> GetAll()
        {
            lock (mutex)
            {
                return entities;
            }
        }

        public GameObjectInstance Replace(int instanceId, int newType)
        {
            lock (mutex)
            {
                var targetObject = entities.FirstOrDefault(x => x.Id == instanceId);
                if (targetObject == null) return null;
                targetObject.Type = newType;
                return targetObject;
            }
        }

        private void RegisterObjectItemDrop(int objectId, params ItemDrop[] items)
        {
            objectItemDrops[objectId] = items;
        }

        protected void AddGameObjects()
        {
            var objects = gameData.GetAllGameObjectInstances(this.gameSession.Id);

            foreach (var obj in objects)
            {
                entities.Add(obj);
            }
        }

        private void AddObjectDrops()
        {
            IReadOnlyList<ItemDrop> drops = gameData.GetAllObjectItemDrops();
            //var drops = objectRepository.GetItemDrops();
            foreach (var drop in drops)
            {
                RegisterObjectItemDrop(drop.EntityId, drop);
            }
        }
    }
}