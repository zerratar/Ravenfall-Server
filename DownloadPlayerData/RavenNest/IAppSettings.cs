namespace DownloadPlayerData
{
    public interface IAppSettings
    {
        string ApiEndpoint { get; }
        string WebSocketEndpoint { get; }
    }
}
