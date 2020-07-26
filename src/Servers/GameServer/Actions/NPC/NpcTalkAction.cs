using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Models;

public class NpcTalkAction : EntityActionInvoker
{
    public NpcTalkAction()
        : base(8, "Npc Talk")
    {
    }

    public override bool Invoke(
        Player player,
        IEntity obj,
        int parameterId)
    {
        return true;
    }
}
