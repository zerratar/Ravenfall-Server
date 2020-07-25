using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class NpcAdd
    {
        public const short OpCode = 27;
        public int ServerId { get; set; }
        public int NpcId { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int Endurance { get; set; }
        public Attributes Attributes { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Destination { get; set; }
        public static NpcAdd Create(IGameData gameData, NpcInstance obj, Transform transform)
        {
            var npc = gameData.GetNpc(obj.NpcId);
            var npcid = npc.NpcId;
            return new NpcAdd
            {
                ServerId = obj.Id,
                NpcId = npcid,
                Level = npc.Level,
                Health = obj.Health,
                Endurance = obj.Endurance,
                Attributes = gameData.GetAttributes(npcid),
                Position = transform.GetPosition(),
                Rotation = transform.GetRotation(),
                Destination = transform.GetDestination()
            };
        }
    }
}
