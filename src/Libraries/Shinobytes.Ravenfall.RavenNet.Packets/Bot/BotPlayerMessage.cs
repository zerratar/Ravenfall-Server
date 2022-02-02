namespace Shinobytes.Ravenfall.RavenNet.Packets.Bot
{
    public class BotPlayerMessage
    {
        public const short OpCode = 1009;
        public string Message { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Session { get; set; }
    }
}
