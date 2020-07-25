using GameServer.Managers;
using GameServer.Processors;
using RavenfallServer.Providers;
using RavenNest.BusinessLogic.Data;

public class FishAction : SkillObjectAction
{
    public FishAction(
        IGameData gameData,
        IWorldProcessor worldProcessor,
        IGameSessionManager sessionManager,
        IPlayerStatsProvider statsProvider,
        IPlayerInventoryProvider inventoryProvider)
: base(3,
      "Fish",
      "Fishing",
      2000,
      gameData,
      worldProcessor,
      sessionManager,
      statsProvider,
      inventoryProvider)
    {
    }
}
