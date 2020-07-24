using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public interface IStreamBot
    {
        int AvailabilityScore { get; }
        void Disconnect(User user);
        void Connect(User user);
        string Name { get; }

        void OnPlayerRemove(string session, Player player);
        void OnPlayerAdd(string session, Player player);
    }
}
