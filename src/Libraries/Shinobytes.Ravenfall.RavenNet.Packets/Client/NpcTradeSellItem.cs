﻿namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class NpcTradeSellItem
    {
        public const short OpCode = 36;
        public int NpcServerId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
    }
}
