using TwitchLib.Client.Models;

namespace ROBot.Core
{
    public interface ITwitchCredentialsProvider
    {
        ConnectionCredentials Get();
    }
}