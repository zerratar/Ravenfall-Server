using GameServer.Managers;
using GameServer.Network;
using GameServer.Processors;
using GameServer.Providers;
using GameServer.Repositories;
using GameServer.Services;
using Microsoft.Extensions.Logging;
using RavenfallServer.Providers;
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.Core.RuleEngine;
using Shinobytes.Ravenfall.Data;
using Shinobytes.Ravenfall.Data.EntityFramework.Legacy;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Serializers;
using Shinobytes.Ravenfall.RavenNet.Server;
using System;
using System.Linq;
using System.Reflection;

namespace GameServer
{
    class Program
    {
        private static IoC RegisterServices()
        {
            var ioc = new IoC();
            ioc.RegisterCustomShared<IoC>(() => ioc);
            ioc.RegisterCustomShared<IAppSettings>(() => new AppSettings
            {
                DbConnectionString = "Server=localhost;Database=Ravenfall2;Trusted_Connection=True;Integrated Security=True;"
            });
            ioc.RegisterShared<IKernel, Kernel>();
            ioc.RegisterShared<ILogger, ConsoleLogger>();
            ioc.RegisterShared<IBinarySerializer, BinarySerializer>();
            ioc.RegisterShared<INetworkPacketTypeRegistry, NetworkPacketTypeRegistry>();
            ioc.RegisterShared<INetworkPacketSerializer, NetworkPacketSerializer>();
            ioc.RegisterShared<IRavenServer, Server>(); // so we can reference this from packet handlers
            ioc.RegisterShared<IServerRegistry, ServerRegistry>();
            ioc.RegisterShared<IMessageBus, MessageBus>();
            ioc.RegisterShared<INetworkPacketController, NetworkPacketController>();

            // providers
            ioc.RegisterShared<IPlayerProvider, PlayerProvider>();
            ioc.RegisterShared<IPlayerStatsProvider, PlayerStatsProvider>();
            ioc.RegisterShared<IPlayerStateProvider, PlayerStateProvider>();
            ioc.RegisterShared<IPlayerInventoryProvider, PlayerInventoryProvider>();


            ioc.RegisterShared<IStreamBotFactory, StreamBotFactory>();

            // Managers
            ioc.RegisterShared<IGameSessionManager, GameSessionManager>();
            ioc.RegisterShared<IUserManager, UserManager>();
            ioc.RegisterShared<IStreamBotManager, StreamBotManager>();

            // db
            ioc.RegisterShared<IRavenfallDbContextProvider, RavenfallDbContextProvider>();
            ioc.RegisterShared<IQueryBuilder, QueryBuilder>();
            ioc.RegisterShared<IGameData, GameData>();

            // IObjectManager and INpcManager should be removed from here
            // they should be instanced per Session
            //ioc.RegisterShared<IObjectManager, ObjectManager>();
            //ioc.RegisterShared<INpcManager, NpcManager>();

            // processors
            ioc.RegisterShared<IWorldProcessor, WorldProcessor>();
            ioc.RegisterShared<IGameSessionProcessor, GameSessionProcessor>();
            ioc.RegisterShared<IPlayerProcessor, PlayerProcessor>();
            ioc.RegisterShared<INpcProcessor, NpcProcessor>();
            ioc.RegisterShared<IObjectProcessor, ObjectProcessor>();

            // Gambit Rule Engine
            ioc.RegisterShared<IGambitRuleGenerator, GambitRuleGenerator>();
            ioc.RegisterShared<IGambitGenerator, GambitGenerator>();
            ioc.RegisterShared<IPlayerGambitRuleProvider, PlayerGambitRuleProvider>();

            // services
            ioc.RegisterShared<IAuthService, AuthService>();

            // repositories
            ioc.RegisterShared<IWorldObjectRepository, JsonBasedWorldObjectRepository>();
            ioc.RegisterShared<IItemRepository, JsonBasedItemRepository>();

            var logger = ioc.Resolve<ILogger>();
            var sessionManager = ioc.Resolve<IGameSessionManager>();
            var packetController = ioc.Resolve<INetworkPacketController>();
            ioc.RegisterCustomShared<IPlayerConnectionProvider>(() =>
                new PlayerConnectionProvider(logger, sessionManager, RegisterPacketHandlers(packetController)));

            return ioc;
        }

        private static INetworkPacketController RegisterPacketHandlers(INetworkPacketController controller)
        {
            var packetHandlers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => !x.IsAbstract && typeof(INetworkPacketHandler).IsAssignableFrom(x));

            foreach (var handler in packetHandlers)
            {
                var packetType = handler.BaseType.GetGenericArguments().FirstOrDefault();
                var packetId = (short)packetType.GetField("OpCode").GetValue(null);
                controller.Register(packetType, handler, packetId);
            }

            return controller;
        }

        static void Main(string[] args)
        {
            Console.Title = "Ravenfall GameServer";

            var ioc = RegisterServices();
            using (var server = ioc.Resolve<IRavenServer>().Start())
            {
                while (true)
                {
                    var str = Console.ReadLine();
                    switch (str)
                    {
                        case "exit":
                        case "quit":
                        case "q":
                            return;
                    }
                }
            }
        }
    }
}
