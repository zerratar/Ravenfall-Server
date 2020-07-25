using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Models;

public class PlayerInspectAction : EntityAction
{
    public PlayerInspectAction()
        : base(4, "Player Inspect")
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
