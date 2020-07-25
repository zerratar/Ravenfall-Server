using GameServer.Managers;
using GameServer.Processors;
using RavenfallServer.Providers;
using RavenNest.BusinessLogic.Data;

public class RockPickAction : SkillObjectAction
{
    public RockPickAction(
        IGameData gameData,
        IWorldProcessor worldProcessor,
        IGameSessionManager sessionManager,
        IPlayerStatsProvider statsProvider,
        IPlayerInventoryProvider inventoryProvider)
        : base(2,
              "RockPick",
              "Mining",
              2000,
              gameData,
              worldProcessor,
              sessionManager,
              statsProvider,
              inventoryProvider)
    {
    }
}
