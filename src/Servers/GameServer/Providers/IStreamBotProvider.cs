namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public interface IStreamBotFactory
    {
        IStreamBot Create(PlayerConnection connection);
    }

}
