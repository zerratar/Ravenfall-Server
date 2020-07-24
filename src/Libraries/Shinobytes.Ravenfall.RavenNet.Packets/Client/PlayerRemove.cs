using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Packets.Client
{
    public class PlayerRemove
    {
        public const short OpCode = 3;
        public int PlayerId { get; set; }

        public static PlayerRemove Create(Player player)
        {
            return new PlayerRemove()
            {
                PlayerId = player.Id
            };
        }
    }
}
