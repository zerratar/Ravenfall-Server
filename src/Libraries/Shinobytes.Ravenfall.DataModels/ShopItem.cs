using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class ShopItem : Entity<ShopItem>
    {
        private int amount;
        private int price;

        public int NpcInstanceId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get => amount; set => Set(ref amount, value); }
        public int Price { get => price; set => Set(ref price, value); }
    }
}