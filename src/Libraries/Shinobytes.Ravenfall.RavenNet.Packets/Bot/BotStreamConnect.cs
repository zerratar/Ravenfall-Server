namespace Shinobytes.Ravenfall.RavenNet.Packets.Bot
{
    public class BotStreamConnect
    {
        public const short OpCode = 1002;
        public string Session { get; set; }
        public string TwitchId { get; set; }
        public string TouTubeId { get; set; }
    }
}
