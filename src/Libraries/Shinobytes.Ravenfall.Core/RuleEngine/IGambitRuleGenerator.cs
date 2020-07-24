using System;

namespace Shinobytes.Ravenfall.Core.RuleEngine
{
    public interface IGambitRuleGenerator
    {
        IGambitRuleAction<TKnowledgeBase> CreateAction<TKnowledgeBase>(Action<TKnowledgeBase> onConditionMet);
        IGambitRuleCondition<TKnowledgeBase> CreateCondition<TKnowledgeBase>(Func<TKnowledgeBase, bool> condition);
        IGambitRule<TKnowledgeBase> CreateRule<TKnowledgeBase>(string name, IGambitRuleCondition<TKnowledgeBase> condition, IGambitRuleAction<TKnowledgeBase> action);
    }
}
