using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Linq;

namespace GameServer.Managers
{
    public class GameSession : IGameSession
    {
        private readonly string sessionKey;

        public GameSession(
            string name,
            INpcManager npcManager,
            IObjectManager objectManager,
            bool isOpenWorldSession)
        {
            this.sessionKey = name;
            Npcs = npcManager;
            Objects = objectManager;
            IsOpenWorldSession = isOpenWorldSession;
            Items = new ItemManager();
            Players = new PlayerManager();
        }

        public IStreamBot Bot { get; private set; }
        public IItemManager Items { get; private set; }
        public INpcManager Npcs { get; private set; }
        public IObjectManager Objects { get; private set; }
        public IPlayerManager Players { get; private set; }
        public PlayerConnection Host { get; private set; }
        public bool IsOpenWorldSession { get; }

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
            }

            AddPlayer(connection.Player);
        }

        public void AddPlayer(Player player)
        {
            player.Session = sessionKey;
            Players.Add(player);

            if (Bot != null)
            {
                Bot.OnPlayerAdd(sessionKey, player);
            }
        }

        public void RemovePlayer(Player player)
        {
            player.Session = null;
            Players.Remove(player);
            Objects.ReleaseLocks(player);

            if (Bot != null)
            {
                Bot.OnPlayerRemove(sessionKey, player);
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
                    Bot.OnPlayerAdd(sessionKey, player);
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