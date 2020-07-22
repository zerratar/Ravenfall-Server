using ROBot.Ravenfall.GameServer;

namespace ROBot
{
    public class RavenfallServerSettings : IRavenfallServerSettings
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string ServerIp { get; set; }

        public int ServerPort { get; set; }
    }
}
