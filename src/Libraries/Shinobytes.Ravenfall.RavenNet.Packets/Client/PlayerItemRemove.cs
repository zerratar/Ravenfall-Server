using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerItemRemove
    {
        public const short OpCode = 18;
        public int PlayerId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }

        public static PlayerItemRemove Create(Player player, Item item, int amount = 1)
        {
            return new PlayerItemRemove
            {
                PlayerId = player.Id,
                ItemId = item.Id,
                Amount = amount
            };
        }
    }
}
