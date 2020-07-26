using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Models;

public abstract class EntityActionInvoker
{
    protected EntityActionInvoker(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }
    public string Name { get; }
    public abstract bool Invoke(Player player, IEntity obj, int parameterId);
}