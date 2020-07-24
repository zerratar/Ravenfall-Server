using TwitchLib.Client.Models;

namespace ROBot.Core.Twitch
{
    public class TwitchCredentialsProvider : ITwitchCredentialsProvider
    {
        private readonly IAppSettings settings;

        public TwitchCredentialsProvider(IAppSettings settings)
        {
            this.settings = settings;
        }

        public ConnectionCredentials Get()
        {
            return new ConnectionCredentials(
                settings.TwitchBotUsername,
                settings.TwitchBotAuthToken);
        }
    }
}