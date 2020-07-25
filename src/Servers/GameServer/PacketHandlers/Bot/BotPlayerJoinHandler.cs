using GameServer.Managers;
using GameServer.Processors;
using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Packets.Bot;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.PacketHandlers
{
    public class BotPlayerJoinHandler : PlayerPacketHandler<BotPlayerJoin>
    {
        private readonly ILogger logger;
        private readonly IWorldProcessor worldProcessor;
        private readonly IPlayerProvider playerProvider;
        private readonly IUserManager userManager;
        private readonly IGameSessionManager sessionManager;

        public BotPlayerJoinHandler(
            ILogger logger,
            IWorldProcessor worldProcessor,
            IPlayerProvider playerProvider,
            IUserManager userManager,
            IGameSessionManager sessionManager)
        {
            this.logger = logger;
            this.worldProcessor = worldProcessor;
            this.playerProvider = playerProvider;
            this.userManager = userManager;
            this.sessionManager = sessionManager;
        }

        protected override void Handle(BotPlayerJoin data, PlayerConnection connection)
        {
            logger.LogDebug($"Player join Request from {connection.Player.Id}. User: {data.Username}, Character: {data.CharacterIndex}");

            var user = userManager.GetByTwitchId(data.TwitchId) ?? userManager.GetByYouTubeId(data.TwitchId) ?? userManager.Get(data.Username);
            if (user == null)
            {
                user = userManager.Create(data.Username, data.TwitchId, data.YouTubeId);
            }
            else
            {
                userManager.LinkSreamerId(user, data.TwitchId, data.YouTubeId);
            }

            var player = playerProvider.Get(user, data.CharacterIndex);
            if (player == null)
            {
                // this user doesnt have any players yet.
                // create one for them.
                player = playerProvider.CreateRandom(user, user.Username);
            }

            var session = sessionManager.Get(data.Session);
            if (session != null)
            {
                if (sessionManager.InSession(user, session))
                {
                    // we cannot join with another player as we are already in
                    // this session.

                    logger.LogDebug($"Cannot add User: {data.Username}, Character: {data.CharacterIndex} to this session as the user already have a player in this session.");
                    return;
                }

                session.AddPlayer(player);
                worldProcessor.AddPlayer(player);
            }
        }
    }
}
