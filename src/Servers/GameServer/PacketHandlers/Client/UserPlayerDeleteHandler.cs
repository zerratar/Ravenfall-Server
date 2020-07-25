﻿using GameServer.Managers;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Linq;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using RavenNest.BusinessLogic.Data;

namespace GameServer.PacketHandlers
{
    public class UserPlayerDeleteHandler : PlayerPacketHandler<UserPlayerDelete>
    {
        private readonly IGameData gameData;

        public UserPlayerDeleteHandler(IGameData gameData)
        {
            this.gameData = gameData;
        }

        protected override void Handle(UserPlayerDelete data, PlayerConnection connection)
        {
            var player = gameData.GetPlayer(data.PlayerId);
            if (player == null)
            {
                return;
            }

            gameData.Remove(gameData.GetAppearance(player.AppearanceId));
            gameData.Remove(gameData.GetTransform(player.TransformId));
            gameData.Remove(gameData.GetAttributes(player.AttributesId));
            gameData.Remove(gameData.GetProfessions(player.ProfessionsId));

            var items = gameData.GetAllPlayerItems(player.Id);
            foreach (var item in items)
            {
                gameData.Remove(item);
            }

            gameData.Remove(player);

            SendPlayerList(connection);
        }

        private void SendPlayerList(PlayerConnection connection)
        {

            var players = gameData.GetPlayers(connection.User).ToArray();
            var appearances = players.Select(x => gameData.GetAppearance(x.AppearanceId)).ToArray();
            connection.Send(UserPlayerList.Create(gameData, players, appearances), SendOption.Reliable);
        }
    }
}
