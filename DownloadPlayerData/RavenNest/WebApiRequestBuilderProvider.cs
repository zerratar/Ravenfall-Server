﻿namespace DownloadPlayerData
{
    public class WebApiRequestBuilderProvider : IApiRequestBuilderProvider
    {
        private readonly IAppSettings settings;
        private readonly ITokenProvider tokenProvider;

        public WebApiRequestBuilderProvider(
            IAppSettings settings,
            ITokenProvider tokenProvider)
        {
            this.settings = settings;
            this.tokenProvider = tokenProvider;
        }

        public IApiRequestBuilder Create()
        {
            return new WebApiRequestBuilder(settings,
                tokenProvider.GetAuthToken(),
                tokenProvider.GetSessionToken());
        }
    }
}
