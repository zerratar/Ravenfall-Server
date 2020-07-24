using Microsoft.Extensions.Logging;
using ROBot.Core.GameServer;
using ROBot.Core.Twitch;
using ROBot.Ravenfall;
using Shinobytes.Ravenfall.RavenNet.Packets.Bot;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace ROBot
{
    public class StreamBotApp : IStreamBotApplication
    {
        private readonly ILogger logger;
        private readonly IRavenfallServerConnection ravenfall;
        private readonly ITwitchCommandClient twitch;
        private bool disposed;

        public StreamBotApp(
            ILogger logger,
            IRavenfallServerConnection ravenfall,
            ITwitchCommandClient twitch
            // IYouTubeCommandClient youtube
            )
        {
            this.logger = logger;
            this.ravenfall = ravenfall;
            this.twitch = twitch;
        }

        public void Run()
        {
            logger.LogInformation("Application Started");

            logger.LogInformation("Establishing Ravenfall Communication..");
            ravenfall.Start();

            logger.LogInformation("Initializing Twitch Integration..");
            twitch.Start();
        }

        public void Shutdown()
        {
            logger.LogInformation("Application Shutdown initialized.");
            Dispose();
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            twitch.Dispose();
            ravenfall.Dispose();
        }

        public void OnPlayerAdd(BotPlayerAdd data)
        {
            twitch.SendChatMessage(data.Session, data.Username + " joined the game.");
        }

        public void OnPlayerRemove(BotPlayerRemove data)
        {
            var session = ravenfall.GetSession(data.Session);
            if (session != null)
            {
                twitch.SendChatMessage(session.Name, data.Username + " left the game.");
            }
        }

        public void BeginSession(BotStreamConnect data)
        {
            ravenfall.BeginSession(data.Session);
            twitch.JoinChannel(data.Session);

            //if (!string.IsNullOrEmpty(data.TwitchId))
            //{

            //    // use the twitch API to get the channel name using the twitch ID                
            //    // data.TwitchId

            //    twitch.JoinChannel(data.Session); // if we have a twitch id
            //}

            //if (!string.IsNullOrEmpty(data.TouTubeId))
            //{
            //    youtube.Join(data.TwitchId);
            //}
        }

        public void EndSession(BotStreamDisconnect data)
        {
            ravenfall.EndSession(data.Session);
            twitch.LeaveChannel(data.Session);

            //if (!string.IsNullOrEmpty(data.TwitchId))
            //{
            //    twitch.LeaveChannel(data.TwitchId);
            //}

            //if (!string.IsNullOrEmpty(data.TouTubeId))
            //{
            //    youtube.Leave(data.TwitchId);
            //}
        }

    }
}
