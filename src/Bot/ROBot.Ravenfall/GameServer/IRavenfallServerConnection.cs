using System;

namespace ROBot.Ravenfall.GameServer
{
    public interface IRavenfallServerConnection : IDisposable
    {
        void Start();
    }
}
