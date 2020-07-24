using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Managers
{
    public interface IPlayerProvider
    {
        Player Get(int playerId);
        bool Remove(int playerId);
        IReadOnlyList<Player> GetAll();
        IReadOnlyList<Player> GetPlayers(User user);
        Player Create(User user, string name, Appearance appearance);
        Player CreateRandom(User user, string name);
        Player Get(User user, int characterIndex);
    }
}
