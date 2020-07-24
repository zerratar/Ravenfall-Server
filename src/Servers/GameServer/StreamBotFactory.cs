using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Threading;
using Shinobytes.Ravenfall.RavenNet.Packets.Bot;

namespace RavenfallServer.Providers
{
    public class StreamBotFactory : IStreamBotFactory
    {
        private readonly ILogger logger;

        public StreamBotFactory(ILogger logger)
        {
            this.logger = logger;
        }

        public IStreamBot Create(PlayerConnection connection, User botUser)
        {
            return new StreamBot(logger, connection, botUser);
        }

        private class StreamBot : IStreamBot
        {
            private readonly ILogger logger;
            private readonly PlayerConnection connection;
            private int connectionCount = 0;
            private readonly User user;

            public string Name => user?.Username;

            public StreamBot(ILogger logger, PlayerConnection connection, User botUser)
            {
                this.logger = logger;
                this.connection = connection;
                this.user = botUser;

                connection.UserTag = user;
                connection.BotTag = this;
            }

            public int AvailabilityScore => Volatile.Read(ref connectionCount);

            public void Connect(User user)
            {
                Interlocked.Increment(ref connectionCount);
                logger.LogDebug("StreamBot connecting to stream: " + user.Username);
                connection.Send(new BotStreamConnect
                {
                    Session = user.Username,
                    TouTubeId = user.YouTubeId,
                    TwitchId = user.TwitchId,
                },
                Shinobytes.Ravenfall.RavenNet.SendOption.Reliable);
            }

            public void Disconnect(User user)
            {
                Interlocked.Decrement(ref connectionCount);
                logger.LogDebug("StreamBot disconnecting from stream: " + user.Username);
                connection.Send(new BotStreamDisconnect
                {
                    Session = user.Username,
                    TouTubeId = user.YouTubeId,
                    TwitchId = user.TwitchId,
                },
                Shinobytes.Ravenfall.RavenNet.SendOption.Reliable);
            }

            public void OnPlayerRemove(string session, Player player)
            {
                connection.Send(new BotPlayerRemove
                {
                    Session = session,
                    PlayerId = player.Id,
                    Username = player.Name
                },
                Shinobytes.Ravenfall.RavenNet.SendOption.Reliable);
            }

            public void OnPlayerAdd(string session, Player player)
            {
                connection.Send(new BotPlayerAdd
                {
                    Session = session,
                    PlayerId = player.Id,
                    Username = player.Name
                },
                Shinobytes.Ravenfall.RavenNet.SendOption.Reliable);
            }
        }
    }
}