namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class UserPlayerSelect
    {
        public const short OpCode = 20;
        public int PlayerId { get; set; }
        public string SessionKey { get; set; }
    }
}
