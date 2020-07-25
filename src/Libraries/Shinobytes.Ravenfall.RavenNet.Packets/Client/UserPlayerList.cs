using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class UserPlayerList
    {
        public const short OpCode = 22;
        public int[] Id { get; set; }
        public string[] Name { get; set; }
        public int[] CombatLevel { get; set; }
        public Appearance[] Appearance { get; set; }
        public SessionInfo[] Session { get; set; }
        public static UserPlayerList Create(IGameData gameData, Player[] players, Appearance[] appearances)
        {
            var ids = new int[players.Length];
            var names = new string[players.Length];
            //var appearances = new Appearance[players.Length];
            var combatLevels = new int[players.Length];
            var sessions = new SessionInfo[players.Length];

            for (var i = 0; i < players.Length; ++i)
            {
                ids[i] = players[i].Id;
                names[i] = players[i].Name;
                //appearances[i] = players[i].Appearance;
                //combatLevels[i] = players[i].CombatLevel;
                sessions[i] = GetSession(gameData, players[i].SessionId);
            }

            return new UserPlayerList
            {
                Id = ids,
                Name = names,
                CombatLevel = combatLevels,
                Appearance = appearances,
                Session = sessions
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
    }
}
