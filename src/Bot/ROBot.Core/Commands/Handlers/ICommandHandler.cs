using System;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace ROBot.Core.Handlers
{
    public interface ITwitchCommandController
    {
        Task HandleAsync(ITwitchCommandClient listener, ChatCommand cmd);

    }
}