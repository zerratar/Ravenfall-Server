using System;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public interface IAuthenticationModule : IModule
    {
        event EventHandler LoginSuccess;
        event EventHandler<LoginFailedEventArgs> LoginFailed;
        bool Authenticated { get; }
        bool Authenticating { get; }
        void Reset();
        void Authenticate(string username, string password);
    }
}
