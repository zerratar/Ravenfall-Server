namespace Shinobytes.Ravenfall.RavenNet.Packets.Bot
{
    public class BotStreamConnect
    {
        public const short OpCode = 1002;
        public string StreamID { get; set; }
    }
}
