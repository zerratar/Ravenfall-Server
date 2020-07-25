
using GameServer.Managers;
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Concurrent;

namespace RavenfallServer.Providers
{
    public class PlayerInventoryProvider : IPlayerInventoryProvider
    {
        private readonly ConcurrentDictionary<int, Inventory> inventories
            = new ConcurrentDictionary<int, Inventory>();

        private readonly IGameData gameData;

        public PlayerInventoryProvider(IGameData gameData)
        {
            this.gameData = gameData;
        }

        public void CreateInventory(int playerId)
        {
            var inventory = inventories[playerId] = new Inventory(playerId, gameData);
            // Add Hatchet and Pickaxe as starting items
            inventory.AddItem(gameData.GetItem(0), 1); // hatchet
            inventory.AddItem(gameData.GetItem(1), 1); // pickaxe
            inventory.AddItem(gameData.GetItem(7), 1); // fishing net
        }

        public Inventory GetInventory(int playerId)
        {
            if (inventories.TryGetValue(playerId, out var inventory))
                return inventory;

            inventory = inventories[playerId] = new Inventory(playerId, gameData);
            return inventory;
        }
    }
}