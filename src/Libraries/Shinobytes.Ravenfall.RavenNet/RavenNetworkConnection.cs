using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Udp;
using System;
using System.Net;

namespace Shinobytes.Ravenfall.RavenNet
{
    public class RavenNetworkConnection : IRavenNetworkConnection, IDisposable
    {
        private readonly INetworkPacketController controller;

        protected readonly ILogger Logger;
        protected UdpConnection Connection;

        private bool disposed;

        public RavenNetworkConnection(
            ILogger logger,
            INetworkPacketController controller)
        {
            this.Logger = logger;
            this.controller = controller;
        }

        public RavenNetworkConnection(
            ILogger logger,
            UdpConnection connection,
            INetworkPacketController packetHandler)
        {
            Logger = logger;
            this.Connection = connection;
            this.controller = packetHandler;
            this.Connection.Disconnected += Connection_Disconnected;
            this.Connection.DataReceived += Connection_DataReceived;
        }

        public event EventHandler Disconnected;
        public event EventHandler Connected;

        public Guid InstanceID => Connection.InstanceID;
        public bool IsConnected => Connection?.State == ConnectionState.Connected;
        public bool IsConnecting => Connection?.State == ConnectionState.Connecting;

        public object UserTag { get; set; }
        public object PlayerTag { get; set; }
        public object BotTag { get; set; }
        public string SessionKey { get; set; }
        public ConnectionState State => Connection.State;

        public void ConnectAsync(IPAddress address, int port)
        {
            CreateConnection(address, port);
            Connection.ConnectAsync();
        }

        public bool Connect(IPAddress address, int port)
        {
            CreateConnection(address, port);
            Connection.Connect();
            return IsConnected;
        }

        private void CreateConnection(IPAddress address, int port)
        {
            if (Connection != null)
            {
                Connection.DataReceived -= Connection_DataReceived;
                Connection.Disconnected -= Connection_Disconnected;
            }

            Connection = new UdpClientConnection(new System.Net.IPEndPoint(address, port));
            Connection.Connected += Connection_Connected;
            Connection.DataReceived += Connection_DataReceived;
            Connection.Disconnected += Connection_Disconnected;
        }

        private void Connection_Connected(object sender, ConnectedEventArgs e)
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }


        private void Connection_DataReceived(DataReceivedEventArgs obj)
        {
            controller.HandlePacketData(this, obj.Message, obj.SendOption);
            //Logger.WriteLine("Packet received");
        }

        public void RequestNonBlocking<TRequest, TResponse>(TRequest request, Func<TResponse, bool> filter)
        {
            controller.AddFilter(filter);
            Send(request, SendOption.Reliable);
        }

        public void Send<TPacket>(short packetId, TPacket packet, SendOption sendOption)
        {
            controller.Send(Connection, packetId, packet, sendOption);
        }

        public void Send<TPacket>(TPacket packet, SendOption sendOption)
        {
            controller.Send(Connection, packet, sendOption);
        }

        public void Disconnect()
        {
            Connection.Disconnect(null);
        }

        private void Connection_Disconnected(object sender, DisconnectedEventArgs e)
        {
            //OnDisconnected();
            Disconnected?.Invoke(this, e);
            this.Dispose();
        }

        //protected abstract void OnDisconnected();

        public void Dispose()
        {
            if (this.disposed) return;
            if (this.Connection != null)
            {
                this.Connection.DataReceived -= Connection_DataReceived;
                this.Connection.Disconnected -= Connection_Disconnected;
                this.Connection.Dispose();
            }
            this.disposed = true;
        }
    }
}
