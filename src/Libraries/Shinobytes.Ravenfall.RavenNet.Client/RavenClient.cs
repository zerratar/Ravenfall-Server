using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using System;
using System.Linq;
using System.Net;

namespace Shinobytes.Ravenfall.RavenNet.Client
{
    public class RavenClient : IRavenClient
    {
        private readonly ILogger logger;
        private readonly object connectionMutex = new object();

        private RavenNetworkConnection client;

        public event EventHandler Disconnected;
        public event EventHandler Connected;

        public IModuleManager Modules { get; }

        private readonly INetworkPacketController packetHandlers;

        public bool IsConnected => client.IsConnected;
        public bool IsConnecting => client.IsConnecting;

        public Modules.IAuthenticationModule Auth { get; private set; }

        public RavenClient(ILogger logger, IModuleManager moduleManager, INetworkPacketController controller)
        {
            this.logger = logger;
            this.Modules = moduleManager;
            this.Auth = this.Modules.AddModule(new Authentication(this));
            this.packetHandlers = RegisterPacketHandlers(controller);
        }

        public void SetAuthModule<T>(Func<IRavenClient, T> factory) 
            where T : Modules.IAuthenticationModule
        {
            this.Modules.RemoveModule(this.Auth);
            this.Auth = this.Modules.AddModule(factory(this));
        }

        private void OnClientConnected(object sender, EventArgs e)
        {
            Connected?.Invoke(this, e);
            OnConnect();
        }

        private void OnClientDisconnected(object sender, EventArgs e)
        {
            Disconnected?.Invoke(this, e);
            OnDisconnect();
        }

        public virtual void OnConnect()
        {
        }

        public virtual void OnDisconnect()
        {
        }

        public virtual void Send<T>(short packetId, T packet, SendOption sendOption)
        {
            client.Send(packetId, packet, sendOption);
        }

        public virtual void Send<T>(T packet, SendOption sendOption)
        {
            client.Send(packet, sendOption);
        }

        public void ConnectAsync(IPAddress address, int port)
        {
            lock (connectionMutex)
            {
                CreateClient();

                client.ConnectAsync(address, port);
            }
        }

        public void Disconnect()
        {
            if (this.client != null)
            {
                this.client.Connected -= this.OnClientConnected;
                this.client.Disconnected -= this.OnClientDisconnected;
                try { client.Disconnect(); } catch { }
                try { client.Dispose(); } catch { }
            }
        }

        public bool Connect(IPAddress address, int port)
        {
            lock (connectionMutex)
            {
                CreateClient();

                if (client.Connect(address, port))
                {
                    logger.LogInformation("Connected to server");
                    return true;
                }

                logger.LogInformation("Unable to connect to server.");
                return false;
            }
        }

        public void Dispose()
        {
            this.client.Connected -= this.OnClientConnected;
            this.client.Disconnected -= this.OnClientDisconnected;

            Modules.Dispose();
            client.Dispose();
        }

        private void CreateClient()
        {
            if (this.client != null)
            {
                this.client.Connected -= this.OnClientConnected;
                this.client.Disconnected -= this.OnClientDisconnected;
            }

            this.client = new RavenNetworkConnection(logger, packetHandlers);
            this.client.Disconnected += OnClientDisconnected;
            this.client.Connected += OnClientConnected;
        }

        private static INetworkPacketController RegisterPacketHandlers(INetworkPacketController controller)
        {
            var packetHandlers = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => !x.IsAbstract && typeof(INetworkPacketHandler).IsAssignableFrom(x))
                .ToArray();

            foreach (var handler in packetHandlers)
            {
                var declaringType = handler.GetInterfaces().OrderByDescending(x => x.FullName).FirstOrDefault();
                var packetType = declaringType.GetGenericArguments().FirstOrDefault();
                var packetId = (short)packetType.GetField("OpCode").GetValue(null);
                controller.Register(packetType, handler, packetId);
            }

            return controller;
        }

    }
}
