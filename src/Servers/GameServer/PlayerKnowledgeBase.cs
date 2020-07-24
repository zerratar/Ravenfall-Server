using GameServer.Managers;
using Shinobytes.Ravenfall.RavenNet.Models;

namespace GameServer
{
    public struct PlayerKnowledgeBase
    {
        public Player Player { get; set; }
        public IGameSession Session { get; set; }
    }
}