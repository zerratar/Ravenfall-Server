namespace Shinobytes.Ravenfall.RavenNet.Packets.Bot
{
    public class BotStreamDisconnect
    {
        public const short OpCode = 1003;
        public string StreamID { get; set; }
    }
}
