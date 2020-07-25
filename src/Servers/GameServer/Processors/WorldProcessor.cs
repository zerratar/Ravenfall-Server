using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GameServer.Managers;
using GameServer.Network;
using Microsoft.Extensions.Logging;
using Shinobytes.Ravenfall.RavenNet.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using GameServer.Repositories;
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.Data.Entities;

namespace GameServer.Processors
{
    public class WorldProcessor : IWorldProcessor
    {
        private readonly ILogger logger;
        private readonly IKernel kernel;
        private readonly IPlayerStatsProvider statsProvider;
        private readonly IPlayerConnectionProvider connectionProvider;
        private readonly IPlayerInventoryProvider playerInventoryProvider;
        private readonly IGameSessionProcessor gameSessionProcessor;
        private readonly IGameSessionManager sessions;
        private readonly IStreamBotManager botManager;
        private readonly IGameData gameData;

        private readonly object objectUpdateMutex = new object();

        private readonly List<EntityTick> entityIntervalItems = new List<EntityTick>();

        public WorldProcessor(
            ILogger logger,
            IKernel kernel,
            IPlayerConnectionProvider connectionProvider,
            IPlayerInventoryProvider playerInventoryProvider,
            IPlayerStatsProvider statsProvider,
            IGameSessionProcessor gameSessionProcessor,
            IGameSessionManager gameSessionManager,
            IStreamBotManager botManager,
            IGameData gameData)
        {
            this.logger = logger;
            this.kernel = kernel;
            this.statsProvider = statsProvider;
            this.playerInventoryProvider = playerInventoryProvider;
            this.connectionProvider = connectionProvider;
            this.gameSessionProcessor = gameSessionProcessor;
            this.sessions = gameSessionManager;
            this.botManager = botManager;
            this.gameData = gameData;
            this.kernel.RegisterTickUpdate(Update, TimeSpan.FromSeconds(1f / 60f));
        }

        public void SendChatMessage(Player player, int channelID, string message)
        {
            var session = sessions.Get(player);
            var connections = connectionProvider.GetConnectedActivePlayerConnections(session);

            // var connections = chatHandler.GetChannelPlayers(channelID);

            foreach (var connection in connections)
            {
                connection.Send(ChatMessage.Create(player, channelID, message), SendOption.Reliable);
            }
        }

        public void UpdateObject(GameObjectInstance obj)
        {
            var session = sessions.Get(obj);
            var gobj = gameData.GetGameObject(obj.ObjectId);
            var transform = gameData.GetTransform(gobj.TransformId);
            foreach (var playerConnection in connectionProvider.GetAllActivePlayerConnections(session))
            {
                playerConnection.Send(ObjectUpdate.Create(obj, transform, gobj.Static), SendOption.Reliable);
            }
        }

        public void AddPlayer(Player player)
        {
            var session = sessions.Get(player);
            var connections = connectionProvider.GetConnectedActivePlayerConnections(session);
            var transform = gameData.GetTransform(player.TransformId);
            var appearance = gameData.GetAppearance(player.AppearanceId);
            var combatLevel = statsProvider.GetCombatLevel(player.Id);

            foreach (var connection in connections)
            {
                connection.Send(PlayerAdd.Create(player, appearance, transform, combatLevel), SendOption.Reliable);
            }
        }

        public void LinkToGameSession(string sessionKey, PlayerConnection myConnection)
        {
            try
            {
                var session = sessions.GetOrCreate(sessionKey);

                session.AddPlayer(myConnection);

                if (!session.IsOpenWorldSession && session.Bot == null)
                {
                    var bot = botManager.GetMostAvailable();
                    if (bot != null)
                    {
                        session.AssignBot(bot);
                    }
                }

                var allPlayers = session.Players.GetAll();
                var connections = connectionProvider.GetConnectedActivePlayerConnections(session);

                var objects = session.Objects.GetAll();
                var npcs = session.Npcs.GetAll();

                var myInventory = playerInventoryProvider.GetInventory(myConnection.Player.Id);
                var myAppearance = gameData.GetAppearance(myConnection.Player.AppearanceId);
                var myTransform = gameData.GetTransform(myConnection.Player.TransformId);
                var myStats = statsProvider.GetStats(myConnection.Player.Id);
                var myCombatLevel = statsProvider.GetCombatLevel(myConnection.Player.Id);

                foreach (var connection in connections)
                {
                    var isMe = connection.InstanceID == myConnection.InstanceID;
                    if (isMe)
                    {
                        connection.Send(MyPlayerAdd.Create(myConnection.Player, myAppearance, myTransform, myCombatLevel, myStats, myInventory.Items), SendOption.Reliable);
                        //connection.Send(PlayerInventory.Create(myConnection.Player, inventory.Items), SendOption.Reliable);
                    }
                    else
                    {
                        var combatLevel = statsProvider.GetCombatLevel(connection.Player.Id);
                        connection.Send(PlayerAdd.Create(myConnection.Player, myAppearance, myTransform, combatLevel), SendOption.Reliable);
                    }
                }

                foreach (var player in allPlayers)
                {
                    if (player.Id == myConnection.Player.Id) continue;
                    var combatLevel = statsProvider.GetCombatLevel(player.Id);
                    var appearance = gameData.GetAppearance(player.AppearanceId);
                    var transform = gameData.GetTransform(player.TransformId);
                    myConnection.Send(PlayerAdd.Create(player, appearance, transform, combatLevel), SendOption.Reliable);
                }

                foreach (var obj in objects)
                {
                    var gobj = gameData.GetGameObject(obj.ObjectId);
                    var transform = gameData.GetTransform(gobj.TransformId);
                    if (gobj.Static)
                    {
                        if (obj.Type != gobj.Type)
                        {
                            myConnection.Send(ObjectUpdate.Create(obj, transform, gobj.Static), SendOption.Reliable);
                        }
                    }
                    else
                    {
                        myConnection.Send(ObjectAdd.Create(obj, transform), SendOption.Reliable);
                    }
                }

                foreach (var npc in npcs)
                {
                    var transform = gameData.GetTransform(npc.TransformId);
                    myConnection.Send(NpcAdd.Create(gameData, npc, transform), SendOption.Reliable);
                }
            }
            catch (Exception exc)
            {
                logger.LogInformation(exc.ToString());
            }
        }

        public void RemovePlayer(Player player)
        {
            var session = sessions.Get(player);
            if (session == null) return;

            session.RemovePlayer(player);

            foreach (var connection in connectionProvider.GetConnectedActivePlayerConnections(session))
            {
                connection.Send(PlayerRemove.Create(player), SendOption.Reliable);
            }
        }

        public void PlayAnimation(Player player, string animation, bool enabled = true, bool trigger = false, int number = 0)
        {
            var session = sessions.Get(player);
            foreach (var connection in connectionProvider.GetConnectedActivePlayerConnections(session))
            {
                connection.Send(PlayerAnimationStateUpdate.Create(player, animation, enabled, trigger, number), SendOption.Reliable);
            }
        }

        public void PlayAnimation(NpcInstance npc, string animation, bool enabled = true, bool trigger = false, int number = 0)
        {
            var session = sessions.Get(npc);
            foreach (var connection in connectionProvider.GetConnectedActivePlayerConnections(session))
            {
                connection.Send(NpcAnimationStateUpdate.Create(npc, animation, enabled, trigger, number), SendOption.Reliable);
            }
        }

        public void UpdatePlayerStat(Player player, EntityStat skill)
        {
            var playerConnection = connectionProvider.GetPlayerConnection(player);
            if (playerConnection != null)
            {
                playerConnection.Send(PlayerStatUpdate.Create(player, skill), SendOption.Reliable);
            }
        }
        public void AddPlayerItem(Player player, Item item, int amount = 1)
        {
            var playerConnection = connectionProvider.GetPlayerConnection(player);
            var inventory = playerInventoryProvider.GetInventory(player.Id);
            if (inventory != null)
            {
                inventory.AddItem(item, amount);
            }

            if (playerConnection != null)
            {
                playerConnection.Send(PlayerItemAdd.Create(player, item, amount), SendOption.Reliable);
            }
        }
        public void RemovePlayerItem(Player player, Item item, int amount = 1)
        {
            var playerConnection = connectionProvider.GetPlayerConnection(player);
            var inventory = playerInventoryProvider.GetInventory(player.Id);
            if (inventory != null)
            {
                inventory.RemoveItem(item, amount);
            }

            if (playerConnection != null)
            {
                playerConnection.Send(PlayerItemRemove.Create(player, item, amount), SendOption.Reliable);
            }
        }


        public void PlayerBuyItem(Player player, NpcInstance npc, int itemId, int amount)
        {
            var session = sessions.Get(player);
            var item = gameData.GetItem(itemId);
            if (item == null) return;

            var shopInventory = session.Npcs.Inventories.GetInventory(npc.Id);
            if (shopInventory == null) return;

            var shopItem = shopInventory.GetItem(itemId);

            var totalPrice = shopItem.Price * amount;
            if (player.Coins < totalPrice) return;

            if (shopInventory.TryRemoveItem(item, amount))
            {
                player.Coins -= totalPrice;

                UpdatePlayerCoins(player);
                AddPlayerItem(player, item, amount);
                NpcTradeUpdateStock(npc);
            }
        }

        public void PlayerSellItem(Player player, NpcInstance npc, int itemId, int amount)
        {
            var session = sessions.Get(player);
            var item = gameData.GetItem(itemId);
            if (item == null) return;

            var inventory = playerInventoryProvider.GetInventory(player.Id);
            if (inventory == null) return;

            var shopInventory = session.Npcs.Inventories.GetInventory(npc.Id);
            if (shopInventory == null) return;

            if (inventory.HasItem(item, amount))
            {
                var shopItem = shopInventory.GetItem(item.Id);
                if (shopItem == null) return;

                //player.Coins += shopItem.Item.Value * amount;
                player.Coins += shopItem.Price * amount;

                RemovePlayerItem(player, item, amount);
                shopInventory.AddItem(item, amount, shopItem.Price);
                NpcTradeUpdateStock(npc);
                UpdatePlayerCoins(player);
            }
        }

        public void UpdatePlayerCoins(Player player)
        {
            var connection = connectionProvider.GetPlayerConnection(player);
            if (connection == null) return;

            connection.Send(new PlayerCoinsUpdate
            {
                Coins = player.Coins,
                PlayerId = connection.Player.Id
            }, SendOption.Reliable);
        }

        public void NpcTradeUpdateStock(NpcInstance npc)
        {
            var session = sessions.Get(npc);
            var shopInventory = session.Npcs.Inventories.GetInventory(npc.Id);
            if (shopInventory == null) return;

            var itemCount = shopInventory.Items.Count;
            var itemIds = new int[itemCount];
            var itemPrice = new int[itemCount];
            var itemStock = new int[itemCount];

            for (var i = 0; i < itemCount; ++i)
            {
                var item = shopInventory.Items[i];
                itemIds[i] = item.ItemId;
                itemPrice[i] = item.Price;
                itemStock[i] = item.Amount;
            }

            foreach (var connection in connectionProvider.GetConnectedActivePlayerConnections(session))
            {
                connection.Send(new NpcTradeUpdateStock
                {
                    ItemId = itemIds,
                    ItemPrice = itemPrice,
                    ItemStock = itemStock,
                    NpcServerId = npc.Id,
                    PlayerId = connection.Player.Id
                }, SendOption.Reliable);
            }
        }

        public void PlayerStatLevelUp(Player player, EntityStat skill, int levelsGained)
        {
            var session = sessions.Get(player);
            foreach (var connection in connectionProvider.GetConnectedActivePlayerConnections(session))
            {
                connection.Send(PlayerLevelUp.Create(player, skill, levelsGained), SendOption.Reliable);
            }
        }

        public void DamageNpc(Player player, NpcInstance npc, int damage, int health, int maxHealth)
        {
            var session = sessions.Get(player);
            foreach (var connection in connectionProvider.GetConnectedActivePlayerConnections(session))
            {
                connection.Send(NpcHealthChange.Create(npc, player, -damage, health, maxHealth), SendOption.Reliable);
            }
        }

        public void KillNpc(Player player, NpcInstance npc)
        {
            var session = sessions.Get(player);
            foreach (var connection in connectionProvider.GetConnectedActivePlayerConnections(session))
            {
                connection.Send(NpcDeath.Create(npc, player), SendOption.Reliable);
            }
        }

        public void RespawnNpc(Player player, NpcInstance npc)
        {
            var session = sessions.Get(player);
            foreach (var connection in connectionProvider.GetConnectedActivePlayerConnections(session))
            {
                connection.Send(NpcRespawn.Create(npc, player), SendOption.Reliable);
            }
        }

        public void SetItemEquipState(Player player, Item item, bool state)
        {
            var session = sessions.Get(player);
            var inventory = playerInventoryProvider.GetInventory(player.Id);
            if (state)
                inventory.EquipItem(item);
            else
                inventory.UnEquipItem(item);

            foreach (var connection in connectionProvider.GetConnectedActivePlayerConnections(session))
            {
                connection.Send(PlayerEquipmentStateUpdate.Create(player, item, state), SendOption.Reliable);
            }
        }

        public void OpenTradeWindow(Player player, NpcInstance npc, string shopName, ShopInventory shopInventory)
        {
            var playerConnection = connectionProvider.GetPlayerConnection(player);
            if (playerConnection == null)
            {
                return;
            }

            var itemCount = shopInventory.Items.Count;
            var itemIds = new int[itemCount];
            var itemPrice = new int[itemCount];
            var itemStock = new int[itemCount];

            for (var i = 0; i < itemCount; ++i)
            {
                var item = shopInventory.Items[i];
                itemIds[i] = item.ItemId;
                itemPrice[i] = item.Price;
                itemStock[i] = item.Amount;
            }

            playerConnection.Send(new NpcTradeOpenWindow
            {
                PlayerId = player.Id,
                ItemId = itemIds,
                ItemPrice = itemPrice,
                ItemStock = itemStock,
                ShopName = shopName,
                NpcServerId = npc.Id,
            }, SendOption.Reliable);
        }

        public void PlayerNpcInteraction(
            Player player, NpcInstance npc, EntityAction action, int parameterId)
        {
            if (action.Invoke(player, npc, parameterId))
            {
                foreach (var playerConnection in connectionProvider.GetAll())
                {
                    playerConnection.Send(new PlayerNpcActionResponse
                    {
                        PlayerId = player.Id,
                        ActionId = action.Id,
                        NpcServerId = npc.Id,
                        ParameterId = parameterId,
                        Status = 1 // 0: fail, 1: success
                    }, SendOption.Reliable);
                }
            }
        }

        public void PlayerObjectInteraction(
            Player player, GameObjectInstance obj, EntityAction action, int parameterId)
        {
            // use 
            if (action.Invoke(player, obj, parameterId))
            {
                foreach (var playerConnection in connectionProvider.GetAll())
                {
                    playerConnection.Send(new PlayerObjectActionResponse
                    {
                        PlayerId = player.Id,
                        ActionId = action.Id,
                        ObjectServerId = obj.Id,
                        ParameterId = parameterId,
                        Status = 1 // 0: fail, 1: success
                    }, SendOption.Reliable);
                }
            }
        }

        public void SetEntityInterval<TObject>(
            Player player, TObject obj, EntityTickHandler<TObject> handleObjectTick)
            where TObject : IEntity
        {
            lock (objectUpdateMutex)
                entityIntervalItems.Add(
                    new EntityTick<TObject>(player, obj, handleObjectTick));
        }
        public ITimeoutHandle SetEntityTimeout<TObject>(
            int milliseconds,
            Player player,
            TObject obj,
            EntityTickHandler<TObject> handleObjectTick)
            where TObject : IEntity
        {
            var tick = new EntityTick<TObject>(player, obj, handleObjectTick);
            var started = DateTime.Now;
            return kernel.SetTimeout(() =>
            {
                var elapsed = DateTime.Now - started;
                tick.Invoke(elapsed);
            }, milliseconds);
        }

        public void ClearEntityTimeout(ITimeoutHandle handle)
        {
            kernel.ClearTimeout(handle);
        }

        private void Update(TimeSpan deltaTime)
        {
            // Server Tick
            var gameSessions = sessions.GetAll();
            foreach (var session in gameSessions)
            {
                gameSessionProcessor.Update(session, deltaTime);
            }

            lock (objectUpdateMutex)
            {
                var ticksToRemove = new List<EntityTick>();
                foreach (var objectTick in entityIntervalItems)
                {
                    if (objectTick.Invoke(deltaTime))
                    {
                        ticksToRemove.Add(objectTick);
                    }
                }

                foreach (var remove in ticksToRemove)
                {
                    entityIntervalItems.Remove(remove);
                }
            }
        }
    }
}