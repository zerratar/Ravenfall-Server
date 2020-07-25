using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Managers
{
    public interface IGameSessionManager
    {
        IReadOnlyList<IGameSession> GetAll();
        IReadOnlyList<IGameSession> GetUserSessions(User user);
        IGameSession Get(NpcInstance npc);
        IGameSession Get(Player player);
        IGameSession Get(GameObjectInstance obj);
        IGameSession Get(string sessionKey);
        IGameSession GetOrCreate(string sessionKey);

        /// <summary>
        ///     Gets all <see cref="IGameSession"/> that has no <see cref="Shinobytes.Ravenfall.RavenNet.Server.IStreamBot"/> monitoring.
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<IGameSession> GetUnmonitoredSessions();
        bool InSession(User user, IGameSession session);
    }
}