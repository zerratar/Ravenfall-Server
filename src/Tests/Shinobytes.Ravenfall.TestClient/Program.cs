using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using Shinobytes.Ravenfall.RavenNet.Serializers;
using System;
using System.Diagnostics;
using System.Net;

namespace Shinobytes.Ravenfall.TestClient
{
    class Program
    {
        private static IoC RegisterServices()
        {
            var ioc = new IoC();
            ioc.RegisterCustomShared<IoC>(() => ioc);
            ioc.RegisterShared<ILogger, ConsoleLogger>();
            ioc.RegisterShared<IBinarySerializer, BinarySerializer>();
            ioc.RegisterShared<INetworkPacketTypeRegistry, NetworkPacketTypeRegistry>();
            ioc.RegisterShared<INetworkPacketSerializer, NetworkPacketSerializer>();
            ioc.RegisterShared<IRavenClient, TestClient>(); // so we can reference this from packet handlers
            ioc.RegisterShared<INetworkPacketController, NetworkPacketController>();
            ioc.RegisterShared<IMessageBus, MessageBus>();
            ioc.RegisterShared<IModuleManager, ModuleManager>();

            return ioc;
        }

        static void Main(string[] args)
        {
            Console.Title = "Ravenfall - Headerless Client";

            const int frontServerPort = 8133;
            var waitForLoginResponse = args.Length > 0;
            var ioc = RegisterServices();
            var logger = ioc.Resolve<ILogger>();

            using (var client = ioc.Resolve<IRavenClient>() as TestClient)
            {
                var ticks = DateTime.UtcNow.Ticks;
                var auth = client.Modules.GetModule<Authentication>();
                var count = 1000;
                var sw = new Stopwatch();
                sw.Start();
                // IPAddress.Loopback
                client.Connect(IPAddress.Loopback, frontServerPort);
                for (var i = 0; i < count; ++i)
                {
                    client.Send(new AuthRequest() { ClientVersion = "TEST", Password = "LUL", Username = "User_" + ticks + "_" + i }, SendOption.Reliable);
                    while (true && waitForLoginResponse)
                    {

                        if (!auth.Authenticated)
                        {
                            if (auth.Authenticating)
                            {
                                System.Threading.Thread.SpinWait(1);
                                continue;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    //client.Send(new PlayerMoveRequest(), SendOption.None);
                    auth.Reset();
                }

                client.Disconnect();

                sw.Stop();

                var packetsPerSecond = client.SentPacketCount / sw.Elapsed.TotalSeconds;
                var packetSpeedMs = TimeSpan.FromSeconds(1d / packetsPerSecond).TotalMilliseconds;
                logger.LogInformation($"@yel@{client.SentPacketCount} packets sent in @whi@{sw.Elapsed.TotalSeconds} @yel@seconds. Avg: @whi@{packetsPerSecond} @yel@per second. @whi@{packetSpeedMs}ms @yel@per packet.");

                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
    }
}
