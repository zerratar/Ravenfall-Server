namespace Shinobytes.Ravenfall.RavenNet.Packets.Bot
{
    public class BotPlayerJoin
    {
        public const short OpCode = 1004;
        public int CharacterIndex { get; set; }
        public string Username { get; set; }
        public string TwitchId { get; set; }
        public string YouTubeId { get; set; }
        public string Session { get; set; }
    }
}
