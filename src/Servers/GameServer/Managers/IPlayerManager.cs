using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Managers
{
    public interface IPlayerManager
    {
        bool Add(Player player);
        bool Remove(Player player);
        IReadOnlyList<Player> GetAll();
        Player Get(User user);
    }
}
