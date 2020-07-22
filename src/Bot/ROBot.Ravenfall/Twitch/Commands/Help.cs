using ROBot.Core;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace ROBot.Ravenfall.TwitchCommands
{
    public class Help : TwitchCommandHandler
    {
        public override Task HandleAsync(ITwitchCommandClient client, ChatCommand cmd)
        {
            var channel = cmd.ChatMessage.Channel;

            client.SendChatMessage(channel, "No help available at this time.");

            return Task.CompletedTask;
        }
    }
}
