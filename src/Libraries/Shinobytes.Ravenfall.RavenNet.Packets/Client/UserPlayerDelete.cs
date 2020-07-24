namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class UserPlayerDelete
    {
        public const short OpCode = 21;
        public int PlayerId { get; set; }
    }
}
