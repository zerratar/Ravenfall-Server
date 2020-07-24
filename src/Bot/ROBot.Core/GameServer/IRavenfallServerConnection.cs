using Shinobytes.Ravenfall.RavenNet;
using System;

namespace ROBot.Core.GameServer
{
    public interface IRavenfallServerConnection : IDisposable
    {
        void Start();
        IGameSession GetSession(string session);
        void BeginSession(string session);
        void EndSession(string session);
        void Send<T>(T packet, SendOption options);
    }
}
