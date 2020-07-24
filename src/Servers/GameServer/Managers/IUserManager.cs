using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;

namespace GameServer.Managers
{
    public interface IUserManager
    {
        User Get(string username);
        User GetByTwitchId(string twitchId);
        User GetByYouTubeId(string youtubeId);
        User Create(string username, string twitchId, string youTubeId, string password = null);
    }
}
