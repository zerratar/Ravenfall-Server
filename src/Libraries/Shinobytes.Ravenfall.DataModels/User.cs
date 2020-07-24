using System.Collections.Generic;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string TwitchId { get; set; }
        public string YouTubeId { get; set; }
        public IReadOnlyList<Player> Players { get; set; }
    }
}