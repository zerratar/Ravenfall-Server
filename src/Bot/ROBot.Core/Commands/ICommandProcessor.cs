
using System;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace ROBot.Core
{
    public abstract class TwitchCommandHandler : ITwitchCommandHandler
    {
        public virtual void Dispose()
        {
        }

        public abstract Task HandleAsync(ITwitchCommandClient broadcaster, ChatCommand cmd);
    }

    public interface ITwitchCommandHandler : IDisposable
    {
        Task HandleAsync(ITwitchCommandClient broadcaster, ChatCommand cmd);
    }
}