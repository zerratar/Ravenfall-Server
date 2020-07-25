using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Providers
{
    public interface INpcStateProvider
    {
        NpcAlignment GetAlignment(Player player, NpcInstance npc);
        bool IsEnemy(Player player, NpcInstance npc);
        void ExitCombat(NpcInstance npc);
        void EnterCombat(NpcInstance npc, Player player);
    }
}