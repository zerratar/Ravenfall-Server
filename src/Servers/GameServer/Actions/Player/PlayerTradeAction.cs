using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Models;

public class PlayerTradeAction : EntityActionInvoker
{
    public PlayerTradeAction()
        : base(5, "Player Trade")
    {
    }

    public override bool Invoke(
        Player player,
        IEntity obj,
        int parameterId)
    {
        return false;
    }
}
