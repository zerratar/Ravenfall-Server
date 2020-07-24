using Shinobytes.Ravenfall.RavenNet.Packets.Bot;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROBot.Ravenfall
{
    public interface IStreamBotApplication : IApplication
    {
        void BeginSession(BotStreamConnect data);
        void EndSession(BotStreamDisconnect data);
    }
}
