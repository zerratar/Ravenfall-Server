using ROBot.Core.Providers;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Packets.Bot;
using System.Collections.Generic;
using System.Linq;

namespace ROBot.Core.GameServer
{
    public class RavenfallGameSession : IGameSession
    {
        private readonly IUserProvider userProvider;
        private readonly IRavenfallServerConnection connection;

        private readonly List<User> users = new List<User>();
        private readonly object mutex = new object();

        public RavenfallGameSession(
            IRavenfallServerConnection connection,
            IUserProvider playerProvider,
            string name)
        {
            this.userProvider = playerProvider;
            this.connection = connection;
            this.Name = name;
        }

        public string Name { get; }

        public void Join(string username, string twitchId, string youtubeId)
        {
            if (AddUser(username, twitchId, youtubeId))
            {
                connection.Send(new BotPlayerJoin
                {
                    Session = Name,
                    Username = username,
                    TwitchId = twitchId,
                    YouTubeId = youtubeId
                }, Shinobytes.Ravenfall.RavenNet.SendOption.Reliable);
            }
        }

        public void Leave(string twitchOrYoutubeId)
        {
            if (RemoveUser(twitchOrYoutubeId, out var user))
            {
                connection.Send(new BotPlayerLeave
                {
                    Session = Name,
                    Username = user.Username
                }, Shinobytes.Ravenfall.RavenNet.SendOption.Reliable);
            }
        }

        public User Get(string userId)
        {
            lock (mutex)
            {
                return users.FirstOrDefault(x => x.TwitchId == userId || x.YouTubeId == userId);
            }
        }

        public bool Contains(string twitchOrYoutubeId)
        {
            return Get(twitchOrYoutubeId) != null;
        }

        public bool Contains(User user)
        {
            lock (mutex)
            {
                return users.Contains(user) || users.FirstOrDefault(x => x.Id == user.Id) != null;
            }
        }

        private bool AddUser(string username, string twitchId, string youtubeId)
        {
            var user = userProvider.Get(username, twitchId, youtubeId);
            lock (mutex)
            {
                if (!Contains(user))
                {
                    users.Add(user);
                    return true;
                }
            }
            return false;
        }

        private bool RemoveUser(string twitchOrYoutubeId, out User removedUser)
        {
            removedUser = null;
            var user = Get(twitchOrYoutubeId);
            if (user != null)
            {
                lock (mutex)
                {
                    removedUser = user;
                    users.Remove(user);
                    return true;
                }
            }
            return false;
        }
    }
}
