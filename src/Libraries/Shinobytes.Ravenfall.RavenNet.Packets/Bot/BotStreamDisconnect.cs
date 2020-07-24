namespace Shinobytes.Ravenfall.RavenNet.Packets.Bot
{
    public class BotStreamDisconnect
    {
        public const short OpCode = 1003;
        public string Session { get; set; }
        public string TwitchId { get; set; }
        public string TouTubeId { get; set; }
    }
}
