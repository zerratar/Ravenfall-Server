using RavenNest.Models;

namespace DownloadPlayerData
{
    public class TokenProvider : ITokenProvider
    {
        private AuthToken authToken;
        private SessionToken sessionToken;

        public AuthToken GetAuthToken() => authToken;

        public SessionToken GetSessionToken() => sessionToken;

        public void SetAuthToken(AuthToken token)
        {
            authToken = token;
        }

        public void SetSessionToken(SessionToken token)
        {
            sessionToken = token;
        }
    }
}
