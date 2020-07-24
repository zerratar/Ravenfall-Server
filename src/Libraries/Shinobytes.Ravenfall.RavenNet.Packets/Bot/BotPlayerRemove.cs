namespace Shinobytes.Ravenfall.RavenNet.Packets.Bot
{
    public class BotPlayerRemove
    {
        public const short OpCode = 1008;
        public int PlayerId { get; set; }
        public string Username { get; set; }
        public string Session { get; set; }
    }
}
