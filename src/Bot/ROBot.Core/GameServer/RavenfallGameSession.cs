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
            AddUser(username, twitchId, youtubeId);
            connection.Send(new BotPlayerJoin
            {
                Session = Name,
                Username = username,
                TwitchId = twitchId,
                YouTubeId = youtubeId
            }, Shinobytes.Ravenfall.RavenNet.SendOption.Reliable);
        }

        public void Leave(string twitchOrYoutubeId)
        {
            var user = Get(twitchOrYoutubeId);
            if (user == null) return;
            connection.Send(new BotPlayerLeave
            {
                Session = Name,
                Username = user.Username
            }, Shinobytes.Ravenfall.RavenNet.SendOption.Reliable);
        }

        public void SendChatMessage(string username, string message)
        {
            var user = GetUserByName(username);
            if (user == null) return;
            connection.Send(new BotPlayerMessage
            {
                Session = Name,
                Username = user.Username,
                Message = message
            }, Shinobytes.Ravenfall.RavenNet.SendOption.Reliable);
        }

        public User GetUserByName(string username)
        {
            lock (mutex)
            {
                return users.FirstOrDefault(x => x.Username.Equals(username, System.StringComparison.OrdinalIgnoreCase));
            }
        }

        public User Get(string userId)
        {
            lock (mutex)
            {
                return users.FirstOrDefault(x => x.TwitchId == userId || x.YouTubeId == userId);
            }
        }
        public User Get(int playerId)
        {
            lock (mutex)
            {
                return users.FirstOrDefault(x => x.Id == playerId);
            }
        }

        public bool ContainsUsername(string username)
        {
            lock (mutex)
            {
                return users.Any(x => x.Username.Equals(username, System.StringComparison.OrdinalIgnoreCase));
            }
        }

        public bool Contains(int playerId)
        {
            lock (mutex)
            {
                return users.Any(x => x.Id == playerId);
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

        public bool AddUser(string username, string twitchId, string youtubeId)
        {
            var user = userProvider.Get(username, twitchId, youtubeId);
            if (user == null) return false;
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

        public bool RemoveUser(int playerId, out User removedUser)
        {
            return RemoveUser(Get(playerId), out removedUser);
        }

        public bool RemoveUser(string twitchOrYoutubeId, out User removedUser)
        {
            return RemoveUser(Get(twitchOrYoutubeId), out removedUser);
        }
        private bool RemoveUser(User user, out User removedUser)
        {
            removedUser = null;
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
