using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Linq;

namespace GameServer.Managers
{
    public class GameSession : IGameSession
    {
        private readonly Session session;

        public GameSession(
            Session session,
            INpcManager npcManager,
            IObjectManager objectManager,
            bool isOpenWorldSession)
        {
            this.session = session;

            Npcs = npcManager;
            Objects = objectManager;
            IsOpenWorldSession = isOpenWorldSession;
            Players = new PlayerManager();
        }

        public IStreamBot Bot { get; private set; }
        public INpcManager Npcs { get; private set; }
        public IObjectManager Objects { get; private set; }
        public IPlayerManager Players { get; private set; }
        public PlayerConnection Host { get; private set; }
        public bool IsOpenWorldSession { get; }
        public int Id => session.Id;

        public bool ContainsPlayer(Player player)
        {
            if (player == null) return false;
            return Players.GetAll().Any(x => x.Id == player.Id);
        }

        public void AddPlayer(PlayerConnection connection)
        {
            if (!IsOpenWorldSession && Host == null)
            {
                Host = connection;
                session.UserId = connection.User.Id;
            }

            AddPlayer(connection.Player);
        }

        public void AddPlayer(Player player)
        {
            player.SessionId = session.Id;
            Players.Add(player);

            if (Bot != null)
            {
                Bot.OnPlayerAdd(session.Name, player);
            }
        }

        public void RemovePlayer(Player player)
        {
            player.SessionId = null;
            Players.Remove(player);
            Objects.ReleaseLocks(player);

            if (Bot != null)
            {
                Bot.OnPlayerRemove(session.Name, player);
            }
        }

        public void AssignBot(IStreamBot bot)
        {
            this.Bot = bot;

            // only player hosted sessions have a host.
            // these are the only sessions that will need the bot to monitor for commands.
            if (Host != null)
            {
                bot.Connect(Host.User);
                foreach (var player in Players.GetAll())
                {
                    Bot.OnPlayerAdd(session.Name, player);
                }
            }
        }

        public void UnassignBot(IStreamBot bot)
        {
            if (Bot != bot)
            {
                return;
            }

            if (Host != null)
            {
                bot.Disconnect(Host.User);
            }

            this.Bot = null;
        }
    }
}