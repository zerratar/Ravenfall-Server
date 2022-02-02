﻿using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Server.Packets;

namespace Shinobytes.Ravenfall.AuthServer.PacketHandlers
{
    public class AuthChallengeHandler : INetworkPacketHandler<AuthChallenge>
    {
        private readonly ILogger logger;

        public AuthChallengeHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public void Handle(AuthChallenge data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.LogDebug("Receiving Auth Request: " + data.Username + " // " + data.Password);

            var authRes = new AuthChallengeResponse()
            {
                CorrelationId = data.CorrelationId,
                SessionKeys = new byte[] { 1, 2, 3, 4 },
                Status = 0
            };

            logger.LogDebug("Sending Auth Response: " + authRes.Status);
            connection.Send(authRes, SendOption.Reliable);
        }
    }
}