namespace Shinobytes.Ravenfall.RavenNet.Packets.Bot
{
    public class BotPlayerAdd
    {
        public const short OpCode = 1007;
        public int PlayerId { get; set; }
        public string Username { get; set; }
        public string Session { get; set; }
    }
}
