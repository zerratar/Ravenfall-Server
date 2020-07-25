using RavenNest.BusinessLogic.Data;

namespace RavenfallServer.Providers
{
    public class PlayerStatsProvider : EntityStatsProvider, IPlayerStatsProvider
    {
        public PlayerStatsProvider(IGameData gameData) : base(gameData, true)
        {
        }
    }
}