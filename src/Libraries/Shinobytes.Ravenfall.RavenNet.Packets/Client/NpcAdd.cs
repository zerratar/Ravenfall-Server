using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class NpcAdd
    {
        public const short OpCode = 27;
        public int ServerId { get; set; }
        public int NpcId { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Destination { get; set; }

        public static NpcAdd Create(IGameData gameData, NpcInstance obj, Transform transform)
        {

            return new NpcAdd
            {
                ServerId = obj.Id,
                NpcId = gameData.GetNpc(obj.NpcId).NpcId,
#warning add health and max health for npc
                Position = transform.GetPosition(),
                Rotation = transform.GetRotation(),
                Destination = transform.GetDestination()
            };
        }
    }
}
