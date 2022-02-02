using RavenNest.Models;

namespace DownloadPlayerData
{
    public interface ITokenProvider
    {
        void SetAuthToken(AuthToken token);
        void SetSessionToken(SessionToken token);
        AuthToken GetAuthToken();
        SessionToken GetSessionToken();
    }
}
