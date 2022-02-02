﻿using GameServer.Managers;
using GameServer.Network;
using GameServer.Services;
using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets.Bot;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using Shinobytes.Ravenfall.RavenNet.Server;
using System;

namespace GameServer.PacketHandlers
{
    public class BotAuthRequestHandler : PlayerPacketHandler<BotAuthRequest>
    {
        private readonly ILogger logger;
        private readonly IUserManager userManager;
        private readonly IAuthService authService;
        private readonly IStreamBotFactory botProvider;
        private readonly IStreamBotManager botManager;

        public BotAuthRequestHandler(
            ILogger logger,
            IUserManager userManager,
            IAuthService authService,
            IStreamBotFactory botProvider,
            IStreamBotManager botManager)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.authService = authService;
            this.botProvider = botProvider;
            this.botManager = botManager;
        }

        protected override void Handle(BotAuthRequest data, PlayerConnection connection)
        {
            logger.LogDebug("Bot Auth Request received. User: " + data.Username + ", Pass: " + data.Password);

            var user = userManager.Get(data.Username);
            if (user == null)
            {
                user = userManager.Create(data.Username, null, null, data.Password);
            }

            var result = authService.Authenticate(user, data.Password);

            logger.LogDebug("Sending Auth Response: @cya@" + (int)result);

            if (result != AuthResult.Success)
            {
                SendFailedLoginResult(connection, result);
                return;
            }

            connection.Disconnected -= ClientDisconnected;
            connection.Disconnected += ClientDisconnected;
            botManager.Add(botProvider.Create(connection, user));

            // authenticated
            // send auth response
            SendSuccessLoginResult(connection);
        }

        private void ClientDisconnected(object sender, EventArgs e)
        {
            var connection = sender as PlayerConnection;
            connection.Disconnected -= ClientDisconnected;
            botManager.Remove(connection.Bot);
        }

        private void SendFailedLoginResult(PlayerConnection connection, AuthResult result)
        {
            connection.Send(new BotAuthResponse()
            {
                Status = (int)result,
                SessionKeys = new byte[0]
            }, SendOption.Reliable);
        }
        private void SendSuccessLoginResult(PlayerConnection connection)
        {
            connection.Send(new BotAuthResponse()
            {
                Status = 0,
                SessionKeys = new byte[4] { 1, 2, 3, 4 }
            }, SendOption.Reliable);
        }
    }
}
