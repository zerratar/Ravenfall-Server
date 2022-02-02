using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROBot.Core
{
    public interface IGameSession
    {
        string Name { get; }
        User Get(string userId);
        User GetUserByName(string username);
        void Join(string username, string twitchId, string youtubeId);
        bool Contains(string userId);
        bool Contains(int playerId);
        bool ContainsUsername(string username);
        void Leave(string userId);
        bool RemoveUser(int playerId, out User removedUser);
        void SendChatMessage(string username, string message);
        bool AddUser(string username, string twitchId, string youtubeId);
    }
}
