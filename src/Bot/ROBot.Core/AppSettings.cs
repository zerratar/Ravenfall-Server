namespace ROBot.Core
{
    public class AppSettings : IAppSettings
    {
        public AppSettings(string twitchBotUsername, string twitchBotAuthToken, string twitchChannel)
        {
            TwitchBotUsername = twitchBotUsername;
            TwitchBotAuthToken = twitchBotAuthToken;
            TwitchChannel = twitchChannel;
        }

        public string TwitchBotUsername { get; }
        public string TwitchBotAuthToken { get; }
        public string TwitchChannel { get; }
    }
}