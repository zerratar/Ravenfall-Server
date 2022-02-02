namespace DownloadPlayerData
{
    public class RavenNestStreamSettings : IAppSettings
    {
        public string ApiEndpoint => "https://ravenfall.stream/api/";
        public string WebSocketEndpoint => "wss://ravenfall.stream/api/stream";
    }
}
