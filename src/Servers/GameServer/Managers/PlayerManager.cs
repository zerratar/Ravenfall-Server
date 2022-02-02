using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Managers
{
    public class PlayerManager : IPlayerManager
    {

        private readonly List<Player> activePlayers = new List<Player>();
        private readonly object mutex = new object();

        public IReadOnlyList<Player> GetAll()
        {
            lock (mutex)
            {
                return activePlayers;
            }
        }

        public bool Add(Player player)
        {
            lock (mutex)
            {
                if (activePlayers.Contains(player))
                    return false;
                activePlayers.Add(player);
                return true;
            }
        }

        public bool Remove(Player player)
        {
            lock (mutex)
            {
                return activePlayers.Remove(player);
            }
        }

        public Player Get(User user)
        {
            lock (mutex)
            {
                return activePlayers.FirstOrDefault(x => x.UserId == user.Id);
            }
        }
    }
}
