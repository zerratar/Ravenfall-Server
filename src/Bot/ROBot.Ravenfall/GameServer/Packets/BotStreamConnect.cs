namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class BotStreamConnect
    {
        public const short OpCode = 1002;
        public string StreamID { get; set; }
    }
}
