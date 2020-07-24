using TwitchLib.Client.Models;

namespace ROBot.Core.Twitch
{
    public interface ITwitchCredentialsProvider
    {
        ConnectionCredentials Get();
    }
}