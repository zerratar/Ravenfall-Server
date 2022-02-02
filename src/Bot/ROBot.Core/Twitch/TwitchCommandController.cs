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
        private const string ChatMessageHandlerName = "MessageHandler";
        private readonly ILogger logger;
        private readonly IoC ioc;

        private readonly ConcurrentDictionary<string, Type> handlerLookup = new ConcurrentDictionary<string, Type>();

        private ITwitchChatMessageHandler messageHandler;

        public TwitchCommandController(
            ILogger logger,
            IoC ioc)
        {
            this.logger = logger;
            this.ioc = ioc;

            RegisterCommandHandlers();
        }

        public async Task HandleAsync(IRavenfallServerConnection game, ITwitchCommandClient twitch, ChatMessage message)
        {
            if (messageHandler == null)
                messageHandler = ioc.Resolve<ITwitchChatMessageHandler>();

            if (messageHandler == null)
            {
                logger.LogInformation("HandleMessage: No message handler available.");
                return;
            }

            await messageHandler.HandleAsync(game, twitch, message);
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
            ioc.RegisterShared<ITwitchChatMessageHandler, TwitchChatMessageHandler>();

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