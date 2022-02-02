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
            if (session != null)
                session.Join(cmd.ChatMessage.Username, cmd.ChatMessage.UserId, null);
            return Task.CompletedTask;
        }
    }
}
