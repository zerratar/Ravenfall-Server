using RavenNest.BusinessLogic.Data;
using System.Collections.Generic;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class ShopInventory
    {
        private readonly int npcInstanceId;
        private readonly IGameData gameData;

        public IReadOnlyList<ShopItem> Items => gameData.GetShopItems(npcInstanceId);

        public ShopInventory(int npcInstanceId, IGameData gameData)
        {
            this.npcInstanceId = npcInstanceId;
            this.gameData = gameData;
        }

        public ShopItem GetItem(int id)
        {
            return gameData.GetShopItem(npcInstanceId, id);
        }

        public bool HasItem(int itemId, int amount)
        {
            var item = GetItem(itemId);
            return item != null && item.Amount >= amount;
        }

        public bool TryRemoveItem(Item item, int amount)
        {
            var i = GetItem(item.Id);
            if (i == null || i.Amount < amount)
                return false;

            RemoveItem(item, amount);
            return true;
        }

        public void AddItem(Item item, int amount, int price)
        {
            var existing = gameData.GetShopItem(npcInstanceId, item.Id);
            if (existing != null)
            {
                existing.Amount += amount;
            }
            else
            {
                var shopItem = gameData.CreateShopItem();
                shopItem.NpcInstanceId = npcInstanceId;
                shopItem.ItemId = item.Id;
                shopItem.Amount = amount;
                shopItem.Price = price;
            }
        }

        public void RemoveItem(Item item, int amount)
        {
            var shopItem = gameData.GetShopItem(npcInstanceId, item.Id);
            if (shopItem == null) return;

            shopItem.Amount -= amount;
            if (shopItem.Amount < 0)
            {
                gameData.Remove(shopItem);
            }
        }
    }
}