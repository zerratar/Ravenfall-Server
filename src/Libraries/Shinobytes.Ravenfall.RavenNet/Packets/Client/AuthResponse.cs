namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class AuthResponse
    {
        public const short OpCode = 1;
        public int Status { get; set; }
        public byte[] SessionKeys { get; set; }
    }
}
