using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class NpcAnimationStateUpdate
    {
        public const short OpCode = 43;
        public int NpcServerId { get; set; }
        public string AnimationState { get; set; }
        public bool Enabled { get; set; }
        public bool Trigger { get; set; }
        public int ActionNumber { get; set; }

        public static NpcAnimationStateUpdate Create(NpcInstance npc, string anim, bool enabled, bool trigger, int number)
        {
            return new NpcAnimationStateUpdate
            {
                NpcServerId = npc.Id,
                AnimationState = anim,
                Enabled = enabled,
                Trigger = trigger,
                ActionNumber = number
            };
        }
    }
}
