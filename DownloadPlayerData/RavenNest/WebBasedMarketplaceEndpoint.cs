using RavenNest.Models;
using System.Threading.Tasks;

namespace DownloadPlayerData
{
    internal class WebBasedMarketplaceEndpoint
    {
        private readonly IApiRequestBuilderProvider request;

        public WebBasedMarketplaceEndpoint(IApiRequestBuilderProvider request)
        {
            this.request = request;
        }

        public Task<MarketItemCollection> GetMarketplaceItems()
        {
            return request.Create()
                .AddParameter("0")
                .AddParameter("3000")
                .Build()
                .SendAsync<MarketItemCollection>(ApiRequestTarget.Marketplace, ApiRequestType.Get);
        }

    }
}
