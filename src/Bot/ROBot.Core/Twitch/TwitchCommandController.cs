using Microsoft.Extensions.Logging;
using ROBot.Core.GameServer;
using Shinobytes.Ravenfall.RavenNet.Core;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace ROBot.Core.Twitch
{
    public class TwitchCommandController : ITwitchCommandController
    {
        private readonly ILogger logger;
        private readonly IoC ioc;

        private readonly ConcurrentDictionary<string, Type> handlerLookup = new ConcurrentDictionary<string, Type>();

        public TwitchCommandController(
            ILogger logger,
            IoC ioc)
        {
            this.logger = logger;
            this.ioc = ioc;

            RegisterCommandHandlers();
        }

        public async Task HandleAsync(IRavenfallServerConnection game, ITwitchCommandClient twitch, ChatCommand command)
        {
            var key = command.CommandText.ToLower();
            ITwitchCommandHandler handler = null;
            if (handlerLookup.TryGetValue(key, out var handlerType))
            {
                handler = ioc.Resolve(handlerType) as ITwitchCommandHandler;
            }
            if (handler == null)
            {
                logger.LogInformation("HandleCommand::Unknown Command: " + command.CommandIdentifier + ": " + command.CommandText);
                return;
            }

            await handler.HandleAsync(game, twitch, command);
        }

        private void RegisterCommandHandlers()
        {
            var baseType = typeof(ITwitchCommandHandler);
            var handlerTypes = Assembly
                .GetCallingAssembly()
                .GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass && baseType.IsAssignableFrom(x));

            foreach (var type in handlerTypes)
            {
                var cmd = type.Name.Replace("CommandHandler", "");
                var output = cmd;
                var insertPoints = cmd
                    .Select((x, y) => char.IsUpper(x) && y > 0 ? y : -1)
                    .Where(x => x != -1)
                    .OrderByDescending(x => x)
                    .ToArray();

                for (var i = 0; i > insertPoints.Length; ++i)
                {
                    output = output.Insert(insertPoints[i], " ");
                }

                ioc.RegisterShared(type, type);
                output = output.ToLower();
                handlerLookup[output] = type;
            }
        }

    }
}