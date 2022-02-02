using RavenNest.Models;
using System.Collections.Generic;

namespace DownloadPlayerData
{
    public class ServerData
    {
        public PagedPlayerCollection PlayerData { get; set; }
        public MarketItemCollection MarketPlaceData { get; set; }
        public IReadOnlyList<UserAccountData> AccountData { get; set; }
    }
}
