using Shinobytes.Ravenfall.Data.Entities;
using System;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class User : Entity<User>
    {
        private string username;
        private string passwordHash;
        private string twitchId;
        private string youTubeId;
        private DateTime created;

        public string Username { get => username; set => Set(ref username, value); }
        public string PasswordHash { get => passwordHash; set => Set(ref passwordHash, value); }
        public string TwitchId { get => twitchId; set => Set(ref twitchId, value); }
        public string YouTubeId { get => youTubeId; set => Set(ref youTubeId, value); }
        public DateTime Created { get => created; set => Set(ref created, value); }
    }

    public class Session : Entity<Session>
    {
        private string name;
        private int userId;
        private DateTime created;

        public string Name { get => name; set => Set(ref name, value); }
        public int UserId { get => userId; set => Set(ref userId, value); }
        public DateTime Created { get => created; set => Set(ref created, value); }
    }
}