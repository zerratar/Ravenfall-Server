using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Models;

public class ExamineAction : EntityAction
{
    public ExamineAction()
        : base(0, "Examine")
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
