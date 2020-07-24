using Shinobytes.Ravenfall.Core.RuleEngine;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Providers
{
    public interface IPlayerGambitRuleProvider
    {
        IReadOnlyList<IGambitRule<PlayerKnowledgeBase>> GetRules(Player player);
    }
}