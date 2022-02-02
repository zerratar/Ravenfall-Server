using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Providers
{
    public interface INpcStateProvider
    {
        NpcAlignment GetAlignment(Player player, NpcInstance npc);
        bool IsEnemy(Player player, NpcInstance npc);
        void ExitCombat(NpcInstance npc);
        bool EnterCombat(NpcInstance npc, Player player);
        bool InCombat(NpcInstance npc, Player opponent);
    }
}