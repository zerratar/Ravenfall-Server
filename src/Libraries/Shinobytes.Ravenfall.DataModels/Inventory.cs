using RavenNest.BusinessLogic.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Inventory
    {
        private readonly int playerId;
        private readonly IGameData gameData;

        public Inventory(int playerId, IGameData gameData)
        {
            this.playerId = playerId;
            this.gameData = gameData;
        }


        public IReadOnlyList<InventoryItem> Items => gameData.GetAllPlayerItems(playerId);

        public InventoryItem GetItem(int id)
        {
            return gameData.GetInventoryItem(playerId, id);
        }

        public InventoryItem GetItemOfType(int interactItemType)
        {
            return Items.FirstOrDefault(x => gameData.GetItem(x.ItemId).Type == interactItemType);
        }

        public void EquipItem(Item item)
        {
        }

        public void UnEquipItem(Item item)
        {
        }

        public bool HasItem(Item item, int amount)
        {
            return HasItem(item.Id, amount);
        }

        public bool HasItem(int itemId, int amount)
        {
            var item = GetItem(itemId);
            return item != null && item.Amount >= amount;
        }

        public void AddItem(Item item, int amount)
        {
            var existing = GetItem(item.Id);
            if (existing != null)
            {
                existing.Amount += amount;
            }
            else
            {
                var inventoryItem = gameData.CreateInventoryItem();
                inventoryItem.ItemId = item.Id;
                inventoryItem.PlayerId = playerId;
                inventoryItem.Amount = amount;
            }
        }

        public void RemoveItem(Item item, int amount)
        {
            var existing = GetItem(item.Id);
            if (existing != null)
            {
                existing.Amount -= amount;
                if (existing.Amount <= 0)
                {
                    gameData.Remove(existing);
                }
            }
        }
    }
}