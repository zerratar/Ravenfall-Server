using GameServer.Managers;
using GameServer.Network;
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
        private readonly IPlayerConnectionProvider connectionProvider;
        private readonly IGameSessionManager sessionManager;

        public BotPlayerJoinHandler(
            ILogger logger,
            IWorldProcessor worldProcessor,
            IPlayerProvider playerProvider,
            IUserManager userManager,
            IPlayerConnectionProvider connectionProvider,
            IGameSessionManager sessionManager)
        {
            this.logger = logger;
            this.worldProcessor = worldProcessor;
            this.playerProvider = playerProvider;
            this.userManager = userManager;
            this.connectionProvider = connectionProvider;
            this.sessionManager = sessionManager;
        }

        protected override void Handle(BotPlayerJoin data, PlayerConnection connection)
        {
            logger.LogDebug($"Player join Request to {connection.User.Username}. User: {data.Username}, Character: {data.CharacterIndex}");

            var user = userManager.GetByTwitchId(data.TwitchId) ?? userManager.GetByYouTubeId(data.TwitchId) ?? userManager.Get(data.Username);
            if (user == null)
            {
                user = userManager.Create(data.Username, data.TwitchId, data.YouTubeId);
            }
            else
            {
                userManager.LinkSreamerId(user, data.TwitchId, data.YouTubeId);
            }

            var joiningPlayer = playerProvider.Get(user, data.CharacterIndex);
            if (joiningPlayer == null)
            {
                // this user doesnt have any players yet.
                // create one for them.
                joiningPlayer = playerProvider.CreateRandom(user, user.Username);
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

                // check if this player is in an active session already.
                // if it is, we cannot join IF that session is hosted by the same user.
                // otherwise remove it from the existing session
                var activePlayerSession = sessionManager.Get(joiningPlayer);
                if (activePlayerSession != null)
                {
                    //if (activePlayerSession.Id == session.Id)
                    //{
                    //    logger.LogDebug($"Cannot add User: {data.Username}, Character: {data.CharacterIndex} to this session as the character is being used in a hosted session.");
                    //    return;
                    //}

                    //var playerConnection = connectionProvider.GetPlayerConnection(joiningPlayer);
                    //if (playerConnection != null && playerConnection.User?.Id == user.Id)
                    //{
                    //    logger.LogDebug($"Cannot add User: {data.Username}, Character: {data.CharacterIndex} to this session as the character is being used in a hosted session.");
                    //    return;
                    //}

                    worldProcessor.RemovePlayer(joiningPlayer);
                }

                session.AddPlayer(joiningPlayer);
                worldProcessor.AddPlayer(joiningPlayer);
            }
        }
    }
}
