namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerCoinsUpdate
    {
        public const short OpCode = 38;
        public int PlayerId { get; set; }
        public long Coins { get; set; }
    }
}
