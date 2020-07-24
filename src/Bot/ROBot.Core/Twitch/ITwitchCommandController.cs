using ROBot.Core.GameServer;
using System;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace ROBot.Core.Twitch
{
    public interface ITwitchCommandController
    {
        Task HandleAsync(IRavenfallServerConnection game, ITwitchCommandClient twitch, ChatCommand cmd);

    }
}