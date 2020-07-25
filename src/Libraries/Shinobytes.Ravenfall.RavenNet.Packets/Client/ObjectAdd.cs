using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class ObjectAdd
    {
        public const short OpCode = 9;
        public int ObjectServerId { get; set; }
        public int ObjectId { get; set; }
        public Vector3 Position { get; set; }

        public static ObjectAdd Create(GameObjectInstance obj, Transform transform)
        {
            return new ObjectAdd
            {
                ObjectServerId = obj.Id,
                ObjectId = obj.Type,
                Position = transform.GetPosition()
            };
        }
    }
}
