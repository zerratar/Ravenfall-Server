using System;
using System.Threading.Tasks;

namespace ROBot.Core.Twitch
{
    public interface ITwitchCommandClient : IDisposable
    {
        void Start();
        void Stop();
        void SendChatMessage(string channel, string message);
        void JoinChannel(string channel);
        void LeaveChannel(string channel);
    }
}