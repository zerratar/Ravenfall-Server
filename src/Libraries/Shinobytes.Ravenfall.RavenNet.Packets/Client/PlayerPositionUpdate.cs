using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerPositionUpdate
    {
        public const short OpCode = 6;
        public Vector3 Position { get; set; }
    }
}
