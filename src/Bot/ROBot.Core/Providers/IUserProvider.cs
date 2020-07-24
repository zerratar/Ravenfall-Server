using Shinobytes.Ravenfall.RavenNet.Models;

namespace ROBot.Core.Providers
{
    public interface IUserProvider
    {
        User GetByName(string username);
        User Get(string userId);
        User Get(string username, string twitchId, string youtubeId);
    }
}