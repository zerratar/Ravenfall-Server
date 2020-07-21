using System;
using System.Collections.Concurrent;

namespace Shinobytes.Ravenfall.DataModels.Legacy
{
    public class SessionState
    {
        public float SyncTime { get; set; }
        public ConcurrentDictionary<Guid, NPCState> NPCStates { get; set; } = new ConcurrentDictionary<Guid, NPCState>();
    }
}