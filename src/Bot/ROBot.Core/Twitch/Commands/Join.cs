using ROBot.Core.GameServer;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace ROBot.Core.Twitch.Commands
{
    public class Join : TwitchCommandHandler
    {
        public override Task HandleAsync(IRavenfallServerConnection game, ITwitchCommandClient twitch, ChatCommand cmd)
        {
            var channel = cmd.ChatMessage.Channel;
            var session = game.GetSession(channel);
            var userId = cmd.ChatMessage.UserId;

            if (session.Contains(userId))
            {
                twitch.SendChatMessage(channel, $"@{cmd.ChatMessage.Username}, you're already playing.");
            }
            else
            {
                session.Join(cmd.ChatMessage.Username, cmd.ChatMessage.UserId, null);
                twitch.SendChatMessage(channel, $"@{cmd.ChatMessage.Username}, joining game...");
            }

            return Task.CompletedTask;
        }
    }
}
