using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class UserPlayerCreate
    {
        public const short OpCode = 19;
        public string Name { get; set; }
        public Appearance Appearance { get; set; }
    }
}
