using ROBot.Core.GameServer;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace ROBot.Core.Twitch.Commands
{
    public class Leave : TwitchCommandHandler
    {
        public override Task HandleAsync(IRavenfallServerConnection game, ITwitchCommandClient twitch, ChatCommand cmd)
        {
            var channel = cmd.ChatMessage.Channel;
            var session = game.GetSession(channel);
            var userId = cmd.ChatMessage.UserId;

            if (session.Contains(userId))
            {
                session.Leave(userId);
                twitch.SendChatMessage(channel, $"@{cmd.ChatMessage.Username}, leaving game...");
            }
            else
            {
                twitch.SendChatMessage(channel, $"@{cmd.ChatMessage.Username}, you're not currently playing. Use !join to start playing.");
            }

            return Task.CompletedTask;
        }
    }
}
