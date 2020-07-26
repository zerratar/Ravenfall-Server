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
            IGameData gameData)
            : base(ioc, gameData)
        {
            this.session = session;
            this.gameData = gameData;
            // Any Npc related stuff should be instanced per Session
            // and should therefor be removed from here.
            Stats = new NpcStatsProvider(gameData);
            States = new NpcStateProvider();
            Inventories = new NpcShopInventoryProvider(gameData);

            AddActions(EntityType.NPC);
        }


        public IReadOnlyList<NpcInstance> GetAll()
        {
            return gameData.GetAllNpcInstances(session.Id);
        }

        public NpcInstance Get(int npcInstanceId)
        {
            return gameData.GetNpcInstance(npcInstanceId);
        }

        public EntityActionInvoker GetAction(NpcInstance npcInstance, int actionId)
        {
            var npc = gameData.GetNpc(npcInstance.NpcId);
            if (entityActions.TryGetValue(npc.NpcId, out var actions))
            {
                return actions.Select(x => x.Value).FirstOrDefault(x => x.Id == actionId);
            }

            return null;
        }
    }
}