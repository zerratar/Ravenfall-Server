using System.Collections.Generic;

namespace Shinobytes.Ravenfall.Core.RuleEngine
{
    public class Gambit<TKnowledgeBase> : IGambit<TKnowledgeBase>
    {
        private readonly List<IGambitRule<TKnowledgeBase>> rules = new List<IGambitRule<TKnowledgeBase>>();
        private readonly object mutex = new object();

        public bool ProcessRules(TKnowledgeBase fact)
        {
            var anyRulesApplied = false;
            foreach (var rule in rules)
            {
                anyRulesApplied = rule.Process(fact) || anyRulesApplied;
            }
            return anyRulesApplied;
        }

        public void AddRule(IGambitRule<TKnowledgeBase> rule)
        {
            lock (mutex) rules.Add(rule);
        }
        public void AddRules(IEnumerable<IGambitRule<TKnowledgeBase>> ruleCollection)
        {
            lock (mutex) rules.AddRange(ruleCollection);
        }

        public void RemoveRule(IGambitRule<TKnowledgeBase> rule)
        {
            lock (mutex) rules.Remove(rule);
        }
    }
}
