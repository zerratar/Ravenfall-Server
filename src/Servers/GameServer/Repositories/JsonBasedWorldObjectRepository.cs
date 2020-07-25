
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Repositories
{
    public class JsonBasedWorldObjectRepository : IWorldObjectRepository
    {
        private readonly JsonBasedRepository<ExtendedGameObject> objectRepo;
        private readonly JsonBasedRepository<ItemDrop> itemDropRepo;

        public JsonBasedWorldObjectRepository()
        {
            objectRepo = new JsonBasedRepository<ExtendedGameObject>();
            itemDropRepo = new JsonBasedRepository<ItemDrop>();
        }

        public IReadOnlyList<ExtendedGameObject> AllObjects()
        {
            return objectRepo.All();
        }

        public IReadOnlyList<ItemDrop> GetItemDrops()
        {
            return itemDropRepo.All();
        }
    }
}
