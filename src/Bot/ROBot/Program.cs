using System;
using ROBot.Core;
using ROBot.Core.Twitch;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Serializers;
using Shinobytes.Ravenfall.RavenNet.Core;
using Microsoft.Extensions.Logging;
using ROBot.Ravenfall;
using ROBot.Core.GameServer;
using Shinobytes.Ravenfall.RavenNet.Client;
using ROBot.Core.Providers;

namespace ROBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var ioc = new IoC();

            ioc.RegisterCustomShared<IoC>(() => ioc);
            ioc.RegisterCustomShared<IAppSettings>(() => new AppSettingsProvider().Get());
            ioc.RegisterCustomShared<IRavenfallServerSettings>(() => new RavenfallServerSettings
            {
                Username = "Ravenfall Official",
                Password = "ROBot is a nice name", // doesnt matter what we use right now. anything is accepted lol.
                ServerIp = "127.0.0.1",
                ServerPort = 8133
            });

            ioc.RegisterShared<ILogger, ConsoleLogger>();
            ioc.RegisterShared<IKernel, Kernel>();
            ioc.RegisterShared<IStreamBotApplication, StreamBotApp>();

            ioc.RegisterShared<IMessageBus, MessageBus>();

            // Ravenfall stuff
            ioc.RegisterShared<IUserProvider, UserProvider>();
            ioc.RegisterShared<IBinarySerializer, BinarySerializer>();
            ioc.RegisterShared<INetworkPacketTypeRegistry, NetworkPacketTypeRegistry>();
            ioc.RegisterShared<INetworkPacketSerializer, NetworkPacketSerializer>();
            ioc.RegisterShared<INetworkPacketController, NetworkPacketController>();
            ioc.RegisterShared<IModuleManager, ModuleManager>();
            ioc.RegisterShared<IRavenClient, RavenClient>();
            ioc.RegisterShared<IRavenfallServerConnection, RavenfallServerConnection>();

            // Twitch stuff
            ioc.RegisterShared<ITwitchUserStore, TwitchUserStore>();
            ioc.RegisterShared<ITwitchCredentialsProvider, TwitchCredentialsProvider>();
            ioc.RegisterShared<ITwitchCommandController, TwitchCommandController>();
            ioc.RegisterShared<ITwitchCommandClient, TwitchCommandClient>();

            // YouTube live stuff
            // ... to be added :)

            var app = ioc.Resolve<IStreamBotApplication>();
            {
                app.Run();
                while (true)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Q)
                    {
                        break;
                    }
                }
                app.Shutdown();
            }
        }
    }
}
