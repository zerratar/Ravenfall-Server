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
        void Join(string username, string twitchId, string youtubeId);
        bool Contains(string userId);
        void Leave(string userId);
    }
}
