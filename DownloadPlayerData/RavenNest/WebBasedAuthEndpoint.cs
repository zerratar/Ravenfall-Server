using RavenNest.Models;
using System.Threading.Tasks;

namespace DownloadPlayerData
{
    internal class WebBasedAuthEndpoint
    {
        private readonly IApiRequestBuilderProvider request;

        public WebBasedAuthEndpoint(IApiRequestBuilderProvider request)
        {
            this.request = request;
        }

        public Task<AuthToken> AuthenticateAsync(string username, string password)
        {
            return request.Create()
                .AddParameter("Username", username)
                .AddParameter("Password", password)
                .Build()
                .SendAsync<AuthToken>(ApiRequestTarget.Auth, ApiRequestType.Post);
        }
    }
}
