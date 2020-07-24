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

        public void BeginSession(BotStreamConnect data)
        {
            ravenfall.BeginSession(data.TwitchId ?? data.TouTubeId);

            if (!string.IsNullOrEmpty(data.TwitchId))
            {
                twitch.JoinChannel(data.TwitchId);
            }
            
            //if (!string.IsNullOrEmpty(data.TouTubeId))
            //{
            //    youtube.Join(data.TwitchId);
            //}
        }

        public void EndSession(BotStreamDisconnect data)
        {
            ravenfall.EndSession(data.TwitchId ?? data.TouTubeId);

            if (!string.IsNullOrEmpty(data.TwitchId))
            { 
                twitch.LeaveChannel(data.TwitchId); 
            }

            //if (!string.IsNullOrEmpty(data.TouTubeId))
            //{
            //    youtube.Leave(data.TwitchId);
            //}
        }
    }
}
