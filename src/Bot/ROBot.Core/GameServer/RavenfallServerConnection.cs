using Microsoft.Extensions.Logging;
using ROBot.Core.Providers;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ROBot.Core.GameServer
{

    public class RavenfallServerConnection : IRavenfallServerConnection
    {
        private readonly ILogger logger;
        private readonly IUserProvider userProvider;
        private readonly IRavenfallServerSettings settings;
        private readonly IRavenClient gameClient;
        private bool disposed;

        private readonly List<IGameSession> sessions = new List<IGameSession>();
        private readonly object sessionMutex = new object();

        public RavenfallServerConnection(
            ILogger logger,
            IUserProvider userProvider,
            IRavenfallServerSettings settings,
            IRavenClient gameClient)
        {
            this.logger = logger;
            this.userProvider = userProvider;
            this.settings = settings;
            this.gameClient = gameClient;

            this.gameClient.SetAuthModule(connection => new BotAuthentication(connection));

            this.gameClient.Auth.LoginFailed += OnLoginFailed;
            this.gameClient.Auth.LoginSuccess += OnLoginSuccess;
            this.gameClient.Connected += OnConnect;
            this.gameClient.Disconnected += OnDisconnect;
        }

        public void Send<T>(T packet, SendOption options)
        {
            this.gameClient.Send(packet, options);
        }

        public IGameSession GetSession(string session)
        {
            lock (sessionMutex)
            {
                return sessions.FirstOrDefault(x => x.Name == session);
            }
        }

        public void BeginSession(string session)
        {
            lock (sessionMutex)
            {
                sessions.Add(new RavenfallGameSession(this, userProvider, session));
            }
        }

        public void EndSession(string session)
        {
            var s = GetSession(session);
            if (s == null) return;

            lock (sessionMutex)
            {
                sessions.Remove(s);
            }
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
        private void OnLoginFailed(object sender, LoginFailedEventArgs e)
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
