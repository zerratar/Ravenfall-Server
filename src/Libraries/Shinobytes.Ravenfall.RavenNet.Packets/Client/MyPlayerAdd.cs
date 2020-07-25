using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class MyPlayerAdd
    {
        public const short OpCode = 16;
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public decimal Experience { get; set; }
        public int Health { get; set; }
        public Vector3 Position { get; set; }
        public Professions Professions { get; set; }
        public Attributes Attributes { get; set; }
        public Appearance Appearance { get; set; }
        public int[] InventoryItemId { get; set; }
        public long[] InventoryItemAmount { get; set; }
        public long Coins { get; set; }

        public static MyPlayerAdd Create(
            IGameData gameData,
            Player player,
            IReadOnlyList<InventoryItem> items)
        {

            var appearance = gameData.GetAppearance(player.AppearanceId);
            var transform = gameData.GetTransform(player.TransformId);
            var attributes = gameData.GetAttributes(player.AttributesId);
            var professions = gameData.GetProfessions(player.ProfessionsId);

            var itemIds = new int[items.Count];
            var amounts = new long[items.Count];
            for (var i = 0; i < items.Count; ++i)
            {
                itemIds[i] = items[i].ItemId;
                amounts[i] = items[i].Amount;
            }

            return new MyPlayerAdd
            {
                PlayerId = player.Id,
                Name = player.Name,
                Experience = player.Experience,
                Level = player.Level,
                Health = player.Health,
                Position = transform.GetPosition(),
                Attributes = attributes,
                Professions = professions,
                Appearance = appearance,
                InventoryItemId = itemIds,
                InventoryItemAmount = amounts,
                Coins = player.Coins
            };
        }
    }
}
