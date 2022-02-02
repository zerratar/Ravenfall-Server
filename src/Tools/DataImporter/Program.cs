using GameServer.Repositories;
using Microsoft.Extensions.Logging;
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.Data;
using Shinobytes.Ravenfall.Data.EntityFramework.Legacy;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Linq;

namespace DataImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger();

            try
            {
                var dbCtxProvider = new RavenfallDbContextProvider(new AppSettings
                {
                    DbConnectionString = "Server=localhost;Database=Ravenfall2;Trusted_Connection=True;Integrated Security=True;"
                });

                IGameData gameData = new GameData(
                    dbCtxProvider,
                    logger,
                    new Kernel(),
                    new QueryBuilder());

                ImportGameObjects(logger, gameData);
            }
            catch (Exception exc)
            {
                logger.LogError(exc.ToString());
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        static void ImportGameObjects(ILogger logger, IGameData gameData)
        {
            var repo = new JsonBasedWorldObjectRepository();
            var objs = repo.AllObjects();

            if (objs.Count > 0)
            {
                logger.LogDebug("Removing existing objects..");
                var objects = gameData.GetAllGameObjects();
                foreach (GameObject obj in objects)
                {
                    var transform = gameData.GetTransform(obj.TransformId);
                    if (transform != null)
                    {
                        gameData.Remove(transform);
                    }

                    gameData.Remove(obj);
                }

                logger.LogDebug("Importing objects from json file..");
                foreach (var obj in objs)
                {
                    var transform = gameData.CreateTransform();
                    transform.SetPosition(obj.Position);
                    transform.SetRotation(obj.Rotation);

                    var newObj = gameData.CreateGameObject();
                    newObj.Id = obj.Id;
                    newObj.Type = obj.Type;
                    newObj.InteractItemType = obj.InteractItemType;
                    newObj.RespawnMilliseconds = obj.RespawnMilliseconds;
                    newObj.Static = obj.Static;
                    newObj.Experience = obj.Experience;
                    newObj.TransformId = transform.Id;
                }
            }

            gameData.Flush();
        }
    }
}

