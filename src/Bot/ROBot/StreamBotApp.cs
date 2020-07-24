using Microsoft.Extensions.Logging;
using ROBot.Core;
using ROBot.Ravenfall;
using ROBot.Ravenfall.GameServer;
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

        public void StreamConnect(BotStreamConnect data)
        {
            twitch.JoinChannel(data.StreamID);
        }

        public void StreamDisconnect(BotStreamDisconnect data)
        {
            twitch.LeaveChannel(data.StreamID);
        }
    }
}
