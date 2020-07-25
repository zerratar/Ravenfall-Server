using GameServer.Repositories;
using RavenfallServer.Providers;
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameServer.Managers
{
    public class PlayerProvider : IPlayerProvider
    {
        private readonly IGameData gameData;
        private readonly IPlayerStatsProvider statsProvider;
        private readonly IPlayerInventoryProvider inventoryProvider;

        public PlayerProvider(
            IGameData gameData,
            IPlayerStatsProvider statsProvider,
            IPlayerInventoryProvider inventoryProvider)
        {
            this.gameData = gameData;
            this.statsProvider = statsProvider;
            this.inventoryProvider = inventoryProvider;
        }

        public Player Get(int playerId)
        {
            return gameData.GetPlayer(playerId);
        }

        public Player Get(User user, int characterIndex)
        {
            var chars = gameData.GetPlayers(user);
            if (chars.Count > characterIndex)
            {
                return chars[characterIndex];
            }
            return null;
        }

        public bool Remove(int playerId)
        {
            var player = Get(playerId);
            if (player == null) return false;
            gameData.Remove(player);
            return true;
        }
        public Player CreateRandom(User user, string name)
        {
            return Create(user, name, gameData.GenerateRandomAppearance());
        }

        public Player Create(User user, string name, Appearance appearance)
        {
            return CreatePlayer(user.Id, name, appearance);
        }

        private Player CreatePlayer(int userId, string playerName, Appearance appearance)
        {
            var random = new Random();
            var pos = new Vector3((float)random.NextDouble() * 2f, 7.5f, (float)random.NextDouble() * 2f);

            if (appearance == null)
            {
                appearance = gameData.GenerateRandomAppearance();
            }
            else
            {
                appearance = gameData.CreateAppearance(appearance);
            }

            var transform = gameData.CreateTransform();
            transform.SetPosition(pos);
            transform.SetDestination(pos);

            var player = gameData.CreatePlayer();
            player.UserId = userId;
            player.Name = playerName;
            player.TransformId = transform.Id;
            player.AppearanceId = appearance.Id;

            inventoryProvider.CreateInventory(player.Id);

            //var addedPlayer = new Player()
            //{
            //    Id = id,
            //    Name = playerName,
            //    UserId = userId,
            //    TransformId = transform.Id,
            //    Created = DateTime.UtcNow,
            //    AppearanceId = appearance.Id
            //};

            //gameData.Add(player);
            return player;
        }

        public IReadOnlyList<Player> GetAll()
        {
            return gameData.GetAllPlayers();
        }

        public IReadOnlyList<Player> GetPlayers(User user)
        {
            return gameData.GetPlayers(user);
        }
    }
}
