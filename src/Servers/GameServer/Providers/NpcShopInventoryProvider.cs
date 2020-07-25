
using GameServer.Managers;
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Concurrent;

namespace RavenfallServer.Providers
{
    public class NpcShopInventoryProvider : INpcShopInventoryProvider
    {
        private readonly ConcurrentDictionary<int, ShopInventory> inventories
          = new ConcurrentDictionary<int, ShopInventory>();
        private readonly IGameData gameData;

        public NpcShopInventoryProvider(IGameData gameData)
        {
            this.gameData = gameData;
        }

        public ShopInventory GetInventory(int npcInstanceId)
        {
            if (inventories.TryGetValue(npcInstanceId, out var inventory))
                return inventory;

            inventory = inventories[npcInstanceId] = new ShopInventory(npcInstanceId, gameData);

            if (inventory.Items.Count == 0)
            {
                // Add Hatchet and Pickaxe as starting items
                inventory.AddItem(gameData.GetItem(0), 1, 10); // hatchet
                inventory.AddItem(gameData.GetItem(1), 1, 10); // pickaxe
                inventory.AddItem(gameData.GetItem(7), 1, 10); // fishing net
                inventory.AddItem(gameData.GetItem(2), 0, 10); // copper ore
                inventory.AddItem(gameData.GetItem(3), 0, 10); // logs
            }

            return inventory;
        }
    }
}