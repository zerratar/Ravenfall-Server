using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using Shinobytes.Ravenfall.RavenNet.Server;
using Shinobytes.Ravenfall.RavenNet.Server.Packets;
using System;

namespace Shinobytes.Ravenfall.FrontServer.PacketHandlers
{
    public class AuthRequestHandler : INetworkPacketHandler<AuthRequest>
    {
        private readonly ILogger logger;
        private readonly IServerRegistry servers;

        public AuthRequestHandler(ILogger logger, IServerRegistry servers)
        {
            this.logger = logger;
            this.servers = servers;
        }

        public void Handle(AuthRequest data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            logger.LogDebug("Auth Request received. User: " + data.Username + ", Pass: " + data.Password + ", ClientVersion: " + data.ClientVersion);


            var ls = servers.GetServerConnection("LoginServer");
            var correlationId = Guid.NewGuid();

            ls.RequestNonBlocking<AuthChallenge, AuthChallengeResponse>(new AuthChallenge()
            {
                ClientVersion = data.ClientVersion,
                CorrelationId = correlationId,
                Password = data.Password,
                Username = data.Username
            }, response =>
            {
                if (response.CorrelationId != correlationId)
                {
                    return false;
                }

                logger.LogDebug("Sending Auth Response: " + response.Status);

                connection.Send(new AuthResponse() { Status = response.Status, SessionKeys = new byte[4] { 1, 2, 3, 4 } }, SendOption.Reliable);

                var gameServer = servers.GetServerConnection("GameServer");

                return true;
            });

        }
    }
}