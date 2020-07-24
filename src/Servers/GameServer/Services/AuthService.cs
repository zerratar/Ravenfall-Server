using Shinobytes.Ravenfall.Core.Security;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace GameServer.Services
{
    public class AuthService : IAuthService
    {
        public AuthResult Authenticate(User user, string password)
        {
            return user?.PasswordHash == StringHasher.Get(password) ? AuthResult.Success : AuthResult.InvalidPassword;
        }
    }
}
