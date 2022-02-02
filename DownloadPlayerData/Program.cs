using RavenNest.DataModels;
using RavenNest.Models;
using Shinobytes.Ravenfall.Data.EntityFramework.Legacy;
using Shinobytes.Ravenfall.RavenNet.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using CharacterState = RavenNest.DataModels.CharacterState;
using InventoryItem = RavenNest.DataModels.InventoryItem;
using MarketItem = RavenNest.DataModels.MarketItem;
using Resources = RavenNest.DataModels.Resources;
using Skills = RavenNest.DataModels.Skills;
using Statistics = RavenNest.DataModels.Statistics;
using SyntyAppearance = RavenNest.DataModels.SyntyAppearance;

namespace DownloadPlayerData
{
    class Program
    {
        private const string UserAccountsFile = @"c:\useraccounts.txt";
        private const string PlayersFile = @"C:\backup\players.json";
        private const string MarketplaceFile = @"C:\backup\marketplace.json";
        private const bool UseLocalData = false;

        static void Main(string[] args)
        {
            var data = DownloadData();

            ExportPlayerData(data);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void ExportPlayerData(ServerData serverData)
        {
            var dbCtxProvider = new RavenfallDbContextProvider(new AppSettings
            {
                DbConnectionString = "Server=BEDROOM;Database=DB_A3551f_ravenfall;Integrated Security=True;"
            });

            var qb = new QueryBuilder();

            var players = serverData.PlayerData.Items;

            var index = 1;

            var sw = new Stopwatch();
            sw.Start();

            using (var con = dbCtxProvider.GetConnection())
            {
                con.Open();
                foreach (var player in players)
                {
                    Console.ResetColor();
                    Console.Write($"[{index:0000}/{players.Count:0000}] Migrating player: " + player.UserName + "... ");

                    var cmdQuery = BuildInsertQuery(serverData, qb, player);
                    var results = 0;
                    foreach (var q in cmdQuery)
                    {
                        using (var cmd = con.CreateCommand())
                        {
                            cmd.CommandText = q;
                            results += cmd.ExecuteNonQuery();
                        }
                    }

                    if (results >= 7)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("OK");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("BAD: " + results + " entries saved. Expected > 7");
                    }

                    Console.ResetColor();
                    ++index;
                }

                con.Close();
                sw.Stop();

                Console.WriteLine("Migration completed in " + sw.Elapsed);
            }
        }

        private static string[] BuildInsertQuery(ServerData serverData, QueryBuilder qb, Player player)
        {
            var queries = new List<string>();
            var query = new StringBuilder();
            User user = Mapper.GetUser(player);

            var marketplaceItems = serverData.MarketPlaceData;
            var userData = serverData.AccountData.FirstOrDefault(x => x.UserId == user.UserId);
            if (userData != null)
            {
                if (user.Id == Guid.Empty)
                    user.Id = userData.Id;
                if (string.IsNullOrEmpty(user.PasswordHash))
                    user.PasswordHash = userData.PasswordHash;
                if (string.IsNullOrEmpty(user.DisplayName))
                    user.DisplayName = userData.DisplayName;
                if (string.IsNullOrEmpty(user.Email))
                    user.Email = userData.Email;
                if (string.IsNullOrEmpty(user.UserName))
                    user.UserName = userData.UserName;
            }

            query.AppendLine(qb.Insert(user));

            Skills skills = Mapper.GetSkills(player);
            query.AppendLine(qb.Insert(skills));

            SyntyAppearance appearance = Mapper.GetAppearance(player);
            query.AppendLine(qb.Insert(appearance));

            Resources resources = Mapper.GetResources(player);
            query.AppendLine(qb.Insert(resources));

            CharacterState state = Mapper.GetState(player);
            query.AppendLine(qb.Insert(state));

            Statistics statistics = Mapper.GetStatistics(player);
            query.AppendLine(qb.Insert(statistics));

            Character character = Mapper.GetCharacter(player, user);
            character.UserId = user.Id;
            character.ResourcesId = resources.Id;
            character.SyntyAppearanceId = appearance.Id;
            character.StateId = state.Id;
            character.StatisticsId = statistics.Id;
            character.SkillsId = skills.Id;

            query.AppendLine(qb.Insert(character));

            queries.Add(query.ToString());
            query.Clear();


            InventoryItem[] inventoryItems = Mapper.GetInventoryItems(player);
            if (inventoryItems.Length > 0)
            {
                foreach (var ii in inventoryItems)
                {
                    ii.CharacterId = character.Id;
                }

                if (inventoryItems.Length > 100)
                {
                    for (var i = 0; i < inventoryItems.Length;)
                    {
                        var take = inventoryItems.Skip(i * 100).Take(100).ToArray();

                        queries.Add(qb.InsertMany(take));

                        i += take.Length;
                    }
                }
                else
                {
                    queries.Add(qb.InsertMany(inventoryItems));
                }
            }

            MarketItem[] marketItems = marketplaceItems.Where(x => x.SellerUserId == user.UserId).Select(Mapper.GetMarketItem).ToArray();
            if (marketItems.Length > 0)
            {
                foreach (var ii in marketItems)
                {
                    ii.SellerCharacterId = character.Id;
                }

                if (marketItems.Length > 100)
                {
                    for (var i = 0; i < marketItems.Length;)
                    {
                        var take = marketItems.Skip(i * 100).Take(100).ToArray();

                        queries.Add(qb.InsertMany(take));

                        i += take.Length;
                    }
                }
                else
                {
                    queries.Add(qb.InsertMany(marketItems));
                }
            }

            return queries.ToArray();
        }

        private static ServerData DownloadData()
        {
            IReadOnlyList<UserAccountData> accountData = new List<UserAccountData>();
            var tokenProvider = new TokenProvider();
            var settings = new RavenNestStreamSettings();
            var apiRequestBuilder = new WebApiRequestBuilderProvider(settings, tokenProvider);
            var authEndPoint = new WebBasedAuthEndpoint(apiRequestBuilder);
            var adminEndPoint = new WebBasedAdminEndpoint(apiRequestBuilder);
            var marketplaceEndpoint = new WebBasedMarketplaceEndpoint(apiRequestBuilder);

            if (!UseLocalData)
            {
                while (true)
                {
                    var password = GetPassword();
                    Console.Write("Authenticating...");
                    var authToken = authEndPoint.AuthenticateAsync("zerratar", password).Result;

                    if (authToken != null)
                    {
                        tokenProvider.SetAuthToken(authToken);
                        Console.WriteLine(" OK");
                        break;
                    }
                    else
                    {
                        Console.WriteLine(" Failed. Try again");
                    }
                }
            }

            if (System.IO.File.Exists(UserAccountsFile))
            {
                Console.Write("Loading user account details from disk... ");
                accountData = UserAccountDataParser.Parse(UserAccountsFile);
                Console.WriteLine("Done");
            }

            Console.Write("Downloading player data... ");
            var players = UseLocalData ? Newtonsoft.Json.JsonConvert.DeserializeObject<PagedPlayerCollection>(System.IO.File.ReadAllText(PlayersFile)) : adminEndPoint.GetPlayersAsync().Result;

            if (!UseLocalData)
            {
                var playerJson = Newtonsoft.Json.JsonConvert.SerializeObject(players);
                System.IO.File.WriteAllText(PlayersFile, playerJson);
            }

            Console.WriteLine("Done");

            Console.Write("Downloading marketplace data... ");
            var items = UseLocalData ? Newtonsoft.Json.JsonConvert.DeserializeObject<MarketItemCollection>(System.IO.File.ReadAllText(MarketplaceFile)) : marketplaceEndpoint.GetMarketplaceItems().Result;
            if (!UseLocalData)
            {
                var itemsJson = Newtonsoft.Json.JsonConvert.SerializeObject(items);
                System.IO.File.WriteAllText(MarketplaceFile, itemsJson);
            }

            Console.WriteLine("Done");

            return new ServerData { PlayerData = players, MarketPlaceData = items, AccountData = accountData };
        }

        private static string GetPassword()
        {
            Console.WriteLine("Enter password");
            ConsoleKeyInfo key;
            string password = "";
            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;

                password += key.KeyChar.ToString();
            }

            return password;
        }
    }
}