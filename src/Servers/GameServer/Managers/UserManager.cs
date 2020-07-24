using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameServer.Managers
{
    public class UserManager : IUserManager
    {
        private readonly List<User> users = new List<User>();
        private readonly object mutex = new object();
        private int userIndex = 0;

        public User Get(string username)
        {
            lock (mutex)
            {
                return users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
        }

        public User GetByTwitchId(string twitchId)
        {
            lock (mutex)
            {
                return users.FirstOrDefault(x => x.TwitchId == twitchId);
            }
        }

        public User GetByYouTubeId(string youtubeId)
        {
            lock (mutex)
            {
                return users.FirstOrDefault(x => x.YouTubeId == youtubeId);
            }
        }

        public User Create(string username, string twitchId, string youTubeId)
        {
            lock (mutex)
            {
                var id = Interlocked.Increment(ref userIndex);
                var addedUser = new User
                {
                    Id = id,
                    TwitchId = twitchId,
                    YouTubeId = youTubeId,
                    Username = username,
                    Players = new Player[0],
                };
                users.Add(addedUser);
                return addedUser;
            }
        }
    }
}
