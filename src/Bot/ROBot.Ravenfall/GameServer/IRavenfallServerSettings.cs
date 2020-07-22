namespace ROBot.Ravenfall.GameServer
{
    public interface IRavenfallServerSettings
    {
        string Username { get; }
        string Password { get; }
        string ServerIp { get; }
        int ServerPort { get; }
    }
}
