using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class ObjectRemove
    {
        public const short OpCode = 10;
        public int ObjectServerId { get; set; }

        public static ObjectRemove Create(WorldObject obj)
        {
            return new ObjectRemove
            {
                ObjectServerId = obj.Id
            };
        }
    }
}
