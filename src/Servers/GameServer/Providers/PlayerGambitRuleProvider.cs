using Shinobytes.Ravenfall.Core.RuleEngine;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Providers
{
    public class PlayerGambitRuleProvider : IPlayerGambitRuleProvider
    {
        public IReadOnlyList<IGambitRule<PlayerKnowledgeBase>> GetRules(Player player)
        {
            return new List<IGambitRule<PlayerKnowledgeBase>>();
        }
    }
}