using ROBot.Core.GameServer;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace ROBot.Core.Twitch.Commands
{
    public class Help : TwitchCommandHandler
    {
        public override Task HandleAsync(IRavenfallServerConnection game, ITwitchCommandClient twitch, ChatCommand cmd)
        {
            var channel = cmd.ChatMessage.Channel;

            twitch.SendChatMessage(channel, "No help available at this time.");

            return Task.CompletedTask;
        }
    }
}
