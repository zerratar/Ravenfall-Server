using Shinobytes.Ravenfall.RavenNet.Modules;
using System;
using System.Net;

namespace Shinobytes.Ravenfall.RavenNet
{
    public interface IRavenClient : IDisposable
    {
        event EventHandler Disconnected;

        event EventHandler Connected;

        bool Connect(IPAddress address, int port);
        void ConnectAsync(IPAddress address, int port);
        void Disconnect();

        IModuleManager Modules { get; }
        Modules.IAuthenticationModule Auth { get; }
        bool IsConnecting { get; }
        bool IsConnected { get; }

        void Send<T>(short packetId, T packet, SendOption sendOption);
        void Send<T>(T packet, SendOption sendOption);
        void SetAuthModule<T>(Func<IRavenClient, T> factory)
            where T : Modules.IAuthenticationModule;
    }
}
