﻿using GameServer.Managers;
using GameServer.Processors;
using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Models;

public class NpcTradeAction : EntityActionInvoker
{
    private readonly IWorldProcessor worldProcessor;
    private readonly IGameSessionManager sessionManager;

    public NpcTradeAction(
        IWorldProcessor worldProcessor,
        IGameSessionManager sessionManager)
        : base(9, "Npc Trade")
    {
        this.worldProcessor = worldProcessor;
        this.sessionManager = sessionManager;
    }

    public override bool Invoke(
        Player player,
        IEntity obj,
        int parameterId)
    {
        if (!(obj is NpcInstance npc))
        {
            return false;
        }

        var session = sessionManager.Get(player);
        var shopInventory = session.Npcs.Inventories.GetInventory(npc.Id);
        if (shopInventory == null)
        {
            return false;
        }

        worldProcessor.SetTarget(player, npc);
        worldProcessor.OpenTradeWindow(player, npc, "Test shop", shopInventory);
        return false;
    }
}
