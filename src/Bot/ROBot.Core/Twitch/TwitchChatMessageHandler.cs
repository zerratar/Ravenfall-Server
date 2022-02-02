using ROBot.Core.GameServer;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace ROBot.Core.Twitch
{
    public class TwitchChatMessageHandler : ITwitchChatMessageHandler
    {
        public void Dispose()
        {
        }

        public Task HandleAsync(IRavenfallServerConnection game, ITwitchCommandClient twitch, ChatMessage msg)
        {
            var channel = msg.Channel;
            var session = game.GetSession(channel);
            if (session != null)
            {
                session.SendChatMessage(msg.Username, msg.Message);
            }
            return Task.CompletedTask;
        }
    }
}