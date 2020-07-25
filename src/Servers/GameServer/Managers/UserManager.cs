using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.Core.Security;
using Shinobytes.Ravenfall.RavenNet.Models;

namespace GameServer.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IGameData gameData;

        public UserManager(IGameData gameData)
        {
            this.gameData = gameData;
        }

        public User Get(string username)
        {
            return gameData.GetUser(username);
        }

        public User GetByTwitchId(string twitchId)
        {
            return gameData.GetUser(twitchId);
        }

        public User GetByYouTubeId(string youtubeId)
        {
            return gameData.GetUser(youtubeId);
        }

        public User Create(string username, string twitchId, string youTubeId, string password = null)
        {
            User user = gameData.CreateUser();
            user.TwitchId = twitchId;
            user.YouTubeId = youTubeId;
            user.Username = username;
            user.PasswordHash = StringHasher.Get(password);
            return user;
        }
    }
}
