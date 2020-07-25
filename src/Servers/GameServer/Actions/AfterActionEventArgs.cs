using Shinobytes.Ravenfall.RavenNet.Models;
using System;

public class AfterActionEventArgs : EventArgs
{
    public Player Player { get; }
    public GameObjectInstance Object { get; }
    public AfterActionEventArgs(Player player, GameObjectInstance obj)
    {
        this.Player = player;
        this.Object = obj;
    }
}
