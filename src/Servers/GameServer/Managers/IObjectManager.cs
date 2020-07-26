using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Managers
{
    public interface IObjectManager
    {
        IReadOnlyList<GameObjectInstance> GetAll();
        GameObjectInstance Replace(int serverObjectId, int newObjectId);
        GameObjectInstance Get(int objectServerId);
        EntityActionInvoker GetAction(GameObjectInstance serverObject, int actionId);
        ItemDrop[] GetItemDrops(GameObjectInstance obj);
        void ReleaseLocks(Player player);
        bool AcquireLock(IEntity obj, Player player);
        bool HasAcquiredLock(IEntity obj, Player player);
    }
}