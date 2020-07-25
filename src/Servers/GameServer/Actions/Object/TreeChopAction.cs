using GameServer.Managers;
using GameServer.Network;
using GameServer.Processors;
using RavenfallServer.Providers;
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;

public class TreeChopAction : SkillObjectAction
{
    private readonly IKernel kernel;

    public TreeChopAction(
        IKernel kernel,
        IGameData gameData,
        IWorldProcessor worldProcessor,
        IGameSessionManager sessionManager,
        IPlayerStatsProvider statsProvider,
        IPlayerInventoryProvider inventoryProvider)
        : base(1,
              "Chop",
              "Woodcutting",
              2000,
              gameData,
              worldProcessor,
              sessionManager,
              statsProvider,
              inventoryProvider)
    {
        this.kernel = kernel;
        AfterAction += (_, ev) => MakeTreeStump(ev.Object);
    }

    private void MakeTreeStump(GameObjectInstance tree)
    {
        tree.Type = 0;
        World.UpdateObject(tree);
        kernel.SetTimeout(() => RespawnTree(tree), GameData.GetGameObject(tree.ObjectId).RespawnMilliseconds);
    }

    private void RespawnTree(GameObjectInstance tree)
    {
        var obj = GameData.GetGameObject(tree.ObjectId);
        tree.Type = obj.Type;
        World.UpdateObject(tree);
    }
}
