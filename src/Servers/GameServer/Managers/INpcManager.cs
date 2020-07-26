using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Managers
{
    public interface INpcManager
    {
        INpcShopInventoryProvider Inventories { get; }
        INpcStatsProvider Stats { get; }
        INpcStateProvider States { get; }

        IReadOnlyList<NpcInstance> GetAll();
        NpcInstance Get(int npcServerId);
        EntityActionInvoker GetAction(NpcInstance npc, int actionId);
    }
}