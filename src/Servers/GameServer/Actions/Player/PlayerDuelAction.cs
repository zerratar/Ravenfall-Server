using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Models;

public class PlayerDuelAction : EntityAction
{
    public PlayerDuelAction()
        : base(6, "Player Duel")
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
