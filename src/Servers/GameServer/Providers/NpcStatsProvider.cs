using RavenNest.BusinessLogic.Data;

namespace RavenfallServer.Providers
{
    public class NpcStatsProvider : EntityStatsProvider, INpcStatsProvider
    {
        public NpcStatsProvider(IGameData gameData) : base(gameData, false)
        {
        }
    }
}