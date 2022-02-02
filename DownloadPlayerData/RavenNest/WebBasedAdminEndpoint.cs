using System.Threading.Tasks;

namespace DownloadPlayerData
{
    internal class WebBasedAdminEndpoint
    {
        private readonly IApiRequestBuilderProvider request;

        public WebBasedAdminEndpoint(IApiRequestBuilderProvider request)
        {
            this.request = request;
        }

        public Task<PagedPlayerCollection> GetPlayersAsync()
        {
            return request.Create()
                .Method("players")
                .AddParameter("0")
                .AddParameter("3000")
                .AddParameter("+UserName")
                .AddParameter("-")
                .Build()
                .SendAsync<PagedPlayerCollection>(ApiRequestTarget.Admin, ApiRequestType.Get);
        }

    }
}
