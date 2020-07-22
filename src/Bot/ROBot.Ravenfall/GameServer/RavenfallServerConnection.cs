using Microsoft.Extensions.Logging;
using ROBot.Core;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ROBot.Ravenfall.GameServer
{
    public class RavenfallServerConnection : IRavenfallServerConnection
    {
        private readonly ILogger logger;
        private readonly IRavenfallServerSettings settings;
        private readonly IRavenClient gameClient;
        private bool disposed;

        public RavenfallServerConnection(
            ILogger logger,
            IRavenfallServerSettings settings,
            IRavenClient gameClient)
        {
            this.logger = logger;
            this.settings = settings;
            this.gameClient = gameClient;
            this.gameClient.Auth.LoginFailed += OnLoginFailed;
            this.gameClient.Auth.LoginSuccess += OnLoginSuccess;
            this.gameClient.Connected += OnConnect;
            this.gameClient.Disconnected += OnDisconnect;
        }

        private void OnLoginFailed(object sender, Authentication.LoginFailedEventArgs e)
        {
            logger.LogError("Bot Login Failed :( ");
        }

        private void OnLoginSuccess(object sender, EventArgs e)
        {
            logger.LogInformation("Bot logged in successefully.");
        }

        private async void OnDisconnect(object sender, EventArgs e)
        {
            logger.LogError("Unable to connect to Ravenfall Game Server. Retrying..");
            await Task.Delay(1000);
            ConnectToServer();
        }

        private void OnConnect(object sender, EventArgs e)
        {
            gameClient.Auth.Authenticate(settings.Username, settings.Password);
        }

        public void Start()
        {
            ConnectToServer();
        }

        public void Dispose()
        {
            if (disposed) return;
            gameClient.Dispose();
            disposed = true;
        }

        private void ConnectToServer()
        {
            gameClient.ConnectAsync(TryGetServerAddress(), settings.ServerPort);
        }

        private IPAddress TryGetServerAddress()
        {
            if (IPAddress.TryParse(settings.ServerIp, out var address))
            {
                return address;
            }

            return IPAddress.Loopback;
        }
    }
}
