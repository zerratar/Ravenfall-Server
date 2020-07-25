using GameServer.Repositories;
using RavenfallServer.Providers;
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Managers
{
    public partial class NpcManager : EntityManager, INpcManager
    {
        private readonly Session session;
        private readonly IGameData gameData;

        public INpcShopInventoryProvider Inventories { get; }

        public INpcStatsProvider Stats { get; }

        public INpcStateProvider States { get; }

        public NpcManager(
            IoC ioc,
            Session session,
            IGameData gameData,
            IEntityActionsRepository actionRepo)
            : base(ioc, actionRepo)
        {
            this.session = session;
            this.gameData = gameData;
            // Any Npc related stuff should be instanced per Session
            // and should therefor be removed from here.
            Stats = new NpcStatsProvider();
            States = new NpcStateProvider();
            Inventories = new NpcShopInventoryProvider(gameData);

            AddActions(EntityType.NPC);
            this.gameData = gameData;
        }


        public IReadOnlyList<NpcInstance> GetAll()
        {
            return gameData.GetAllNpcInstances(session.Id);
        }

        public NpcInstance Get(int npcInstanceId)
        {
            return gameData.GetNpcInstance(npcInstanceId);
        }

        public EntityAction GetAction(NpcInstance npc, int actionId)
        {
            if (entityActions.TryGetValue(npc.NpcId, out var actions))
            {
                return actions.Select(x => x.Value).FirstOrDefault(x => x.Id == actionId);
            }

            return null;
        }
    }
}