using RavenfallServer.Providers;
using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Concurrent;

namespace GameServer.Providers
{
    public abstract class EntityStateProvider : IEntityStateProvider
    {
        protected readonly ConcurrentDictionary<string, object> State
             = new ConcurrentDictionary<string, object>();
        public void RemoveState(IEntity entity, string key)
        {
            State.TryRemove(entity.Id + key, out _);
        }

        public T GetState<T>(IEntity entity, string key)
        {
            var rowKey = entity.Id + key;
            if (State.TryGetValue(rowKey, out var value))
            {
                return (T)value;
            }
            return default;
        }

        public void SetState<T>(IEntity entity, string key, T value)
        {
            var rowKey = entity.Id + key;
            State[rowKey] = value;
        }
    }
}