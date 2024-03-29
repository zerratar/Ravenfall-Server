﻿using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerMoveResponse
    {
        public const short OpCode = 5;
        public int PlayerId { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
        public bool Running { get; set; }
    }
}
