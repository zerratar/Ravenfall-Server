using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class ObjectUpdate
    {
        public const short OpCode = 11;
        public int ObjectServerId { get; set; }
        public int ObjectId { get; set; }
        public Vector3 Position { get; set; }
        public bool Static { get; set; }
        public static ObjectUpdate Create(GameObjectInstance obj, Transform transform, bool isStatic)
        {
            return new ObjectUpdate
            {
                ObjectServerId = obj.Id,
                ObjectId = obj.Type,
                Position = transform.GetPosition(),
                Static = isStatic
            };
        }
    }
}
