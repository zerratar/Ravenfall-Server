using GameServer.Repositories;
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Managers
{
    public abstract class EntityManager
    {
        protected readonly ConcurrentDictionary<int, Lazy<EntityActionInvoker>[]> entityActions
            = new ConcurrentDictionary<int, Lazy<EntityActionInvoker>[]>();

        private ConcurrentDictionary<string, Type> loadedActionTypes;

        // Both objectPlayerLocks and playerObjectLocks are used for bidirectional lookups
        // objectPlayerLocks: Key: objectId, Value: player
        private readonly ConcurrentDictionary<int, Player> entityPlayerLocks = new ConcurrentDictionary<int, Player>();
        // playerObjectLocks: Key: playerId, Value: objectId
        private readonly ConcurrentDictionary<int, int> playerEntityLocks = new ConcurrentDictionary<int, int>();
        private readonly IoC ioc;
        private readonly IGameData gameData;

        public EntityManager(IoC ioc, IGameData gameData)
        {
            this.ioc = ioc;
            this.gameData = gameData;
        }

        public bool AcquireLock(IEntity obj, Player player)
        {
            if (entityPlayerLocks.TryGetValue(obj.Id, out var ownedPlayer))
            {
                return ownedPlayer.Id == player.Id;
            }

            playerEntityLocks[player.Id] = obj.Id;
            entityPlayerLocks[obj.Id] = player;
            return true;
        }

        public bool HasAcquiredLock(IEntity obj, Player player)
        {
            if (entityPlayerLocks.TryGetValue(obj.Id, out var ownedPlayer))
            {
                return ownedPlayer.Id == player.Id;
            }

            return false;
        }

        public void ReleaseLocks(Player player)
        {
            if (playerEntityLocks.TryGetValue(player.Id, out var entityId))
            {
                playerEntityLocks.TryRemove(player.Id, out _);
                entityPlayerLocks.TryRemove(entityId, out _);
            }
        }

        protected void AddActions(EntityType type)
        {
            var actions = gameData.GetActions(type);
            foreach (var action in actions)
            {
                Type[] actionTypes = ResolveActionTypes(action.Action);
                RegisterActions(action.EntityId, actionTypes);
            }
        }

        protected Type[] ResolveActionTypes(params string[] actionTypes)
        {
            if (loadedActionTypes == null)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                loadedActionTypes = new ConcurrentDictionary<string, Type>(
                    assemblies
                    .SelectMany(x => x.GetTypes().Where(x => typeof(EntityActionInvoker).IsAssignableFrom(x)))
                    .ToDictionary(x => x.FullName, y => y));
            }

            return actionTypes
                .Select(x => loadedActionTypes.TryGetValue(x, out var type) ? type : null)
                .ToArray();
        }

        private void RegisterActions(int entityId, params Type[] actionTypes)
        {
            var actions = new List<Lazy<EntityActionInvoker>>();
            foreach (var type in actionTypes)
            {
                ioc.RegisterShared(type, type);
                actions.Add(new Lazy<EntityActionInvoker>(() => (EntityActionInvoker)ioc.Resolve(type)));
            }

            entityActions[entityId] = actions.ToArray();
        }

    }
}