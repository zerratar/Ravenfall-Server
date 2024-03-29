﻿using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerAnimationStateUpdate
    {
        public const short OpCode = 12;
        public int PlayerId { get; set; }
        public string AnimationState { get; set; }
        public bool Enabled { get; set; }
        public bool Trigger { get; set; }
        public int ActionNumber { get; set; }

        public static PlayerAnimationStateUpdate Create(Player player, string anim, bool enabled, bool trigger, int number)
        {
            return new PlayerAnimationStateUpdate
            {
                PlayerId = player.Id,
                AnimationState = anim,
                Enabled = enabled,
                Trigger = trigger,
                ActionNumber = number
            };
        }
    }
}
