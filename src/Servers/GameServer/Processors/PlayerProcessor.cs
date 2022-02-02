using GameServer.Managers;
using GameServer.Providers;
using GameServer.Repositories;
using Shinobytes.Ravenfall.Core.RuleEngine;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace GameServer.Processors
{
    public class PlayerProcessor : IPlayerProcessor
    {
        private readonly IPlayerGambitRuleProvider ruleProvider;
        private readonly IGambitGenerator engineGenerator;
        private readonly IGambitRuleGenerator ruleGenerator;
        public PlayerProcessor(
            IPlayerGambitRuleProvider ruleProvider,
            IGambitGenerator engineGenerator,
            IGambitRuleGenerator ruleGenerator)
        {
            this.ruleProvider = ruleProvider;
            this.engineGenerator = engineGenerator;
            this.ruleGenerator = ruleGenerator;
        }

        public void Update(Player player, IGameSession gameSession, TimeSpan deltaTime)
        {
            var rules = ruleProvider.GetRules(player);
            if (rules != null && rules.Count > 0)
            {
                var engine = this.engineGenerator.CreateEngine<PlayerKnowledgeBase>();
                engine.AddRules(rules);
                engine.ProcessRules(new PlayerKnowledgeBase() { Player = player, Session = gameSession });
            }

        }
    }
}