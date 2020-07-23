using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ROBot.Core;
using ROBot.Core.Handlers;
using Shinobytes.Ravenfall.RavenNet.Core;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Enums;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;

namespace ROBot
{
    public class TwitchCommandClient : ITwitchCommandClient
    {
        private readonly ILogger logger;
        private readonly IKernel kernel;
        private readonly ITwitchCommandController commandHandler;
        private readonly ITwitchCredentialsProvider credentialsProvider;

        private TwitchClient client;
        private bool isInitialized;
        private int reconnectDelay = 10000;
        private bool tryToReconnect = true;
        private bool disposed;

        public TwitchCommandClient(
            ILogger logger,
            IKernel kernel,
            ITwitchCommandController commandHandler,
            ITwitchCredentialsProvider credentialsProvider)
        {
            this.logger = logger;
            this.kernel = kernel;
            this.commandHandler = commandHandler;
            this.credentialsProvider = credentialsProvider;
            this.CreateTwitchClient();
        }

        public void Start()
        {
            if (!kernel.Started) kernel.Start();
            EnsureInitialized();
            Subscribe();
            client.Connect();
        }

        public void Stop()
        {
            if (kernel.Started) kernel.Stop();
            Unsubscribe();

            tryToReconnect = false;
            if (client.IsConnected)
                client.Disconnect();
        }

        public void SendChatMessage(string channel, string message)
        {
            client.SendMessage(channel, message);
        }

        public void JoinChannel(string channel)
        {
            if (!string.IsNullOrEmpty(channel))
            {
                logger.LogDebug("Trying to join a channel without a name.");
                return;
            }

            client.JoinChannel(channel);
        }

        public void LeaveChannel(string channel)
        {
            if (!string.IsNullOrEmpty(channel))
            {
                logger.LogDebug("Trying to leave a channel without a name.");
                return;
            }

            client.LeaveChannel(channel);
        }

        private void OnUserLeft(object sender, OnUserLeftArgs e)
        {
        }

        private void OnUserJoined(object sender, OnUserJoinedArgs e)
        {
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
        }

        private async void OnCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            await commandHandler.HandleAsync(this, e.Command);
        }

        private void OnReSub(object sender, OnReSubscriberArgs e)
        {
        }

        private void OnNewSub(object sender, OnNewSubscriberArgs e)
        {
        }

        private void OnPrimeSub(object sender, OnCommunitySubscriptionArgs e)
        {
        }

        private void OnGiftedSub(object sender, OnGiftedSubscriptionArgs e)
        {
        }

        private void OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            logger.LogInformation("Disconnected from the Twitch IRC Server");
            TryToReconnect();
        }

        private void EnsureInitialized()
        {
            if (isInitialized) return;
            client.Initialize(credentialsProvider.Get());
            isInitialized = true;
        }

        private void CreateTwitchClient()
        {
            client = new TwitchClient(new TcpClient(new ClientOptions { ClientType = ClientType.Chat }));
        }

        private void TryToReconnect()
        {
            try
            {
                Unsubscribe();
                isInitialized = false;
                CreateTwitchClient();
                Start();
            }
            catch (Exception)
            {
                logger.LogInformation($"Failed to reconnect to the Twitch IRC Server. Retry in {reconnectDelay}ms");
                Task.Run(async () =>
                {
                    await Task.Delay(reconnectDelay);

                    if (!tryToReconnect)
                        return;

                    TryToReconnect();
                });
            }
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            logger.LogDebug("Connected to Twitch IRC Server");
        }

        private void OnReconnected(object sender, OnReconnectedEventArgs e)
        {
            logger.LogDebug("Reconnected to Twitch IRC Server");
        }

        private void OnRaidNotification(object sender, OnRaidNotificationArgs e)
        {
        }

        private void Subscribe()
        {
            client.OnChatCommandReceived += OnCommandReceived;
            client.OnMessageReceived += OnMessageReceived;
            client.OnConnected += OnConnected;
            client.OnReconnected += OnReconnected;
            client.OnDisconnected += OnDisconnected;
            client.OnUserJoined += OnUserJoined;
            client.OnUserLeft += OnUserLeft;
            client.OnGiftedSubscription += OnGiftedSub;
            client.OnCommunitySubscription += OnPrimeSub;
            client.OnNewSubscriber += OnNewSub;
            client.OnReSubscriber += OnReSub;
            client.OnRaidNotification += OnRaidNotification;
        }

        private void Unsubscribe()
        {
            client.OnChatCommandReceived -= OnCommandReceived;
            client.OnMessageReceived -= OnMessageReceived;
            client.OnConnected -= OnConnected;
            client.OnDisconnected -= OnDisconnected;
            client.OnUserJoined -= OnUserJoined;
            client.OnUserLeft -= OnUserLeft;
            client.OnGiftedSubscription -= OnGiftedSub;
            client.OnCommunitySubscription -= OnPrimeSub;
            client.OnNewSubscriber -= OnNewSub;
            client.OnReSubscriber -= OnReSub;
            client.OnRaidNotification -= OnRaidNotification;
        }

        public void Dispose()
        {
            if (disposed)
                return;
            Unsubscribe();
            Stop();
            disposed = true;
        }
    }
}