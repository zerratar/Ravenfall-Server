using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Linq;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class UserPlayerList
    {
        public const short OpCode = 22;
        public SelectablePlayer[] Players { get; set; }
        public static UserPlayerList Create(IGameData gameData, Player[] players, Appearance[] appearances)
        {
            return new UserPlayerList
            {
                Players = players.Select((player, index) => new SelectablePlayer
                {
                    Appearance = appearances[index],
                    Id = player.Id,
                    Level = player.Level,
                    Name = player.Name,
                    Session = GetSession(gameData, player.SessionId)
                }).ToArray()
            };
        }

        private static SessionInfo GetSession(IGameData gameData, int? sessionId)
        {
            if (sessionId == null) return new SessionInfo();

            var session = gameData.GetSession(sessionId.Value);
            if (session == null) return new SessionInfo();

            return new SessionInfo
            {
                Id = session.Id,
                Name = session.Name
            };
        }

        public class SessionInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class SelectablePlayer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Level { get; set; }
            public Appearance Appearance { get; set; }
            public SessionInfo Session { get; set; }
        }
    }

}
