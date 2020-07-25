using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ROBot.Core.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly List<User> users = new List<User>();
        private readonly object mutex = new object();
        private int tempUserIndex = 0;

        public User Get(string username, string twitchId, string youtubeId)
        {
            var user = GetByName(username) ?? Get(twitchId) ?? Get(youtubeId);
            if (user != null)
            {
                return user;
            }

            lock (mutex)
            {
                user = new User()
                {
                    Id = Interlocked.Increment(ref tempUserIndex),
                    Username = username,
                    TwitchId = twitchId,
                    YouTubeId = youtubeId
                };
                users.Add(user);
            }

            return user;
        }

        public User Get(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            lock (mutex)
            {
                return users.FirstOrDefault(x => x.TwitchId == userId || x.YouTubeId == userId);
            }
        }


        public User GetByName(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            lock (mutex)
            {
                return users.FirstOrDefault(x => x.Username == username);
            }
        }
    }
}
