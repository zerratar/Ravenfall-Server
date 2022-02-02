using GameServer.Repositories;
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Managers
{
    public class GameSessionManager : IGameSessionManager
    {
        private const string OpenWorldGameSessionKey = "$__OPEN_WORLD__$";
        private readonly ConcurrentDictionary<string, IGameSession> gameSessions = new ConcurrentDictionary<string, IGameSession>();
        private readonly IoC ioc;
        private readonly IGameData gameData;


        public GameSessionManager(
            IoC ioc,
            IGameData gameData)
        {
            this.ioc = ioc;
            this.gameData = gameData;
        }

        public IReadOnlyList<IGameSession> GetUserSessions(User user)
        {
            return gameSessions.Values.Where(session => session.Players.GetAll().Any(x => x.UserId == user.Id)).ToList();
        }

        public bool InSession(User user, IGameSession session)
        {
            return gameSessions.Values.FirstOrDefault(session => session.Players.GetAll().Any(x => x.UserId == user.Id)) != null;
        }

        public IGameSession Get(NpcInstance npc)
        {
            return gameSessions.Values.FirstOrDefault(session => session.Id == npc.SessionId);
        }

        public IGameSession Get(GameObjectInstance obj)
        {
            return gameSessions.Values.FirstOrDefault(session => session.Id == obj.SessionId);
        }

        public IGameSession Get(Player player)
        {
            return gameSessions.Values.FirstOrDefault(session => session.ContainsPlayer(player));
        }

        public IGameSession Get(string sessionKey)
        {
            if (string.IsNullOrEmpty(sessionKey))
            {
                sessionKey = OpenWorldGameSessionKey;
            }

            if (gameSessions.TryGetValue(sessionKey, out var session))
            {
                return session;
            }
            return null;
        }

        public IGameSession GetOrCreate(string sessionKey)
        {
            if (string.IsNullOrEmpty(sessionKey))
            {
                sessionKey = OpenWorldGameSessionKey;
            }

            if (gameSessions.TryGetValue(sessionKey, out var session))
            {
                return session;
            }

            var isOpenWorldSession = sessionKey == OpenWorldGameSessionKey;
            return gameSessions[sessionKey] = CreateGameSession(sessionKey, isOpenWorldSession);
        }

        public IReadOnlyList<IGameSession> GetAll()
        {
            return gameSessions.Values.ToList();
        }

        public IReadOnlyList<IGameSession> GetUnmonitoredSessions()
        {
            return gameSessions.Values.Where(x => x.Bot == null).ToList();
        }

        private IGameSession CreateGameSession(string sessionKey, bool isOpenWorldSession)
        {
            var session = gameData.GetSession(sessionKey);
            if (session == null)
            {
                session = gameData.CreateSession();
                session.Name = sessionKey;

                foreach (var npc in gameData.GetAllNpcs())
                {
                    var srcTransform = gameData.GetTransform(npc.TransformId);
                    var transform = gameData.CreateTransform();
                    transform.SetPosition(srcTransform.GetPosition());
                    transform.SetRotation(srcTransform.GetRotation());
                    transform.SetDestination(srcTransform.GetDestination());

                    var npcInstance = gameData.CreateNpcInstance();
                    npcInstance.SessionId = session.Id;
                    npcInstance.Alignment = npc.Alignment;
                    npcInstance.NpcId = npc.Id;
                    npcInstance.TransformId = transform.Id;
                }

                foreach (var obj in gameData.GetAllGameObjects())
                {
                    var newObj = gameData.CreateGameObjectInstance();
                    newObj.SessionId = session.Id;
                    newObj.Type = obj.Type;
                    newObj.ObjectId = obj.Id;
                }
            }

            var npcs = new NpcManager(ioc, session, gameData);
            var objects = new ObjectManager(ioc, session, gameData);
            var gameSession = new GameSession(session, npcs, objects, isOpenWorldSession);
            return gameSession;
        }

    }
}