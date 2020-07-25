using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Models;

public class PlayerAttackAction : EntityAction
{
    public PlayerAttackAction()
        : base(7, "Player Attack")
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
