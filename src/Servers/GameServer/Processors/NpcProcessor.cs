using GameServer.Managers;
using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace GameServer.Processors
{
    // note(zerratar): keep this processor stateless for sessions
    //                 as it is being used for processing all npcs in all sessions.
    public class NpcProcessor : INpcProcessor
    {
        private readonly ILogger logger;
        private readonly IKernel kernel;

        public NpcProcessor(
            ILogger logger,
            IKernel kernel)
        {
            this.logger = logger;
            this.kernel = kernel;
        }

        public void Update(NpcInstance npc, IGameSession session, TimeSpan deltaTime)
        {
        }
    }
}