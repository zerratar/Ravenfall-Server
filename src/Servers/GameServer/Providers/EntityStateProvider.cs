using RavenfallServer.Providers;
using Shinobytes.Ravenfall.Data.Entities;
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

        public bool SetState<T>(IEntity entity, string key, T value)
        {
            var rowKey = entity.Id + key;
            State.TryGetValue(rowKey, out var val);
            State[rowKey] = value;

            try
            {
                return !object.Equals(val, value);
            }
            catch
            {
                return true;
            }
        }
    }
}