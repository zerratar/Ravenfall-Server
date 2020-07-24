namespace Shinobytes.Ravenfall.RavenNet.Packets.Bot
{
    public class BotPlayerTrain
    {
        public const short OpCode = 1006;
        public int CharacterIndex { get; set; }
        public string Username { get; set; }
        public string Session { get; set; }
        public string Skill { get; set; }
    }
}
