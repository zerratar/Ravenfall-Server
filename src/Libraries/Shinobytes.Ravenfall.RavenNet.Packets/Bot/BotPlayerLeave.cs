namespace Shinobytes.Ravenfall.RavenNet.Packets.Bot
{
    public class BotPlayerLeave
    {
        public const short OpCode = 1005;
        public string Username { get; set; }
        public string Session { get; set; }
    }
}
