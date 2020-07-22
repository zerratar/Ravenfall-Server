namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class AuthRequest
    {
        public const short OpCode = 0;
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientVersion { get; set; }
    }
}
