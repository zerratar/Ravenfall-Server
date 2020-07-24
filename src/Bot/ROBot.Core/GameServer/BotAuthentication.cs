using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets.Bot;

namespace ROBot.Core.GameServer
{
    public class BotAuthentication : Authentication
    {
        public BotAuthentication(IRavenClient connection) : base(connection)
        {
        }

        protected override void SendAuthRequest(IRavenClient client, string username, string password)
        {
            client.Send(new BotAuthRequest { Username = username, Password = password }, SendOption.Reliable);
        }
    }
}
