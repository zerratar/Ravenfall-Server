using GameServer;
using RavenNest.BusinessLogic;
using RavenNest.DataModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DownloadPlayerData
{
    public class Mapper
    {
        private static string GetHexColor(HairColor color)
        {
            switch (color)
            {
                case HairColor.Blonde:
                    return "#A8912A";
                case HairColor.Blue:
                    return "#0D9BB9";
                case HairColor.Brown:
                    return "#3C2823";
                case HairColor.Grey:
                    return "#595959";
                case HairColor.Pink:
                    return "#DF62C7";
                case HairColor.Red:
                    return "#C52A4A";
                default:
                    return "#000000";
            }
        }

        private static string GetHexColor(SkinColor color)
        {
            switch (color)
            {
                case SkinColor.Light:
                    return "#d6b8ae";
                case SkinColor.Medium:
                    return "#faa276";
                default:
                    return "#40251e";
            }
        }

        private static SyntyAppearance GenerateRandomSyntyAppearance()
        {
            var gender = Utility.Random<Gender>();
            var skinColor = GetHexColor(Utility.Random<SkinColor>());
            var hairColor = GetHexColor(Utility.Random<HairColor>());
            var beardColor = GetHexColor(Utility.Random<HairColor>());
            return new SyntyAppearance
            {
                Id = Guid.NewGuid(),
                Gender = gender,
                SkinColor = skinColor,
                HairColor = hairColor,
                BeardColor = beardColor,
                StubbleColor = skinColor,
                WarPaintColor = hairColor,
                EyeColor = "#000000",
                Eyebrows = Utility.Random(0, gender == Gender.Male ? 10 : 7),
                Hair = Utility.Random(0, 38),
                FacialHair = gender == Gender.Male ? Utility.Random(0, 18) : -1,
                Head = Utility.Random(0, 23),
                HelmetVisible = true
            };
        }

        public static Skills GetSkills(Player player)
        {
            var skill = DataMapper.Map<Skills, RavenNest.Models.Skills>(player.Skills);
            skill.Id = Guid.NewGuid();
            return skill;
        }

        public static Character GetCharacter(Player player, User user)
        {
            return new Character
            {
                Id = Guid.NewGuid(),
                Name = player.Name,
                AppearanceId = player.Appearance.Id,
                ResourcesId = player.Resources.Id,
                SkillsId = player.Skills.Id,
                StateId = player.State.Id,
                StatisticsId = player.Statistics.Id,
                SyntyAppearanceId = player.Appearance.Id,
                UserId = user.Id,
                Created = DateTime.UtcNow,
            };
        }

        internal static SyntyAppearance GetAppearance(Player player)
        {
            var app = DataMapper.Map<SyntyAppearance, RavenNest.Models.SyntyAppearance>(player.Appearance);
            if (app.Id == Guid.Empty)
            {
                app = GenerateRandomSyntyAppearance();
            }
            app.Id = Guid.NewGuid();
            return app;
        }

        internal static Resources GetResources(Player player)
        {
            var res = DataMapper.Map<Resources, RavenNest.Models.Resources>(player.Resources);

            res.Id = Guid.NewGuid();
            return res;
        }

        internal static CharacterState GetState(Player player)
        {
            var state = DataMapper.Map<CharacterState, RavenNest.Models.CharacterState>(player.State);

            state.Id = Guid.NewGuid();
            return state;
        }

        internal static InventoryItem[] GetInventoryItems(Player player)
        {
            var items = player.InventoryItems.Select(x => Map(x)).ToArray();
            return MergeItems(items);
        }

        private static InventoryItem[] MergeItems(InventoryItem[] items)
        {
            List<InventoryItem> output = new List<InventoryItem>();

            foreach (var item in items.GroupBy(x => x.ItemId))
            {
                var typeList = item.ToList();
                var characterId = typeList[0].CharacterId;
                var itemId = typeList[0].ItemId;

                var equipped = typeList.FirstOrDefault(x => x.Equipped);
                var total = typeList.Sum(x => x.Amount);
                if (equipped != null)
                {
                    output.Add(new InventoryItem
                    {
                        CharacterId = equipped.CharacterId,
                        Equipped = true,
                        Amount = 1,
                        Id = Guid.NewGuid(),
                        ItemId = equipped.ItemId
                    });
                    --total;
                }
                if (total > 0)
                {
                    output.Add(new InventoryItem
                    {
                        Amount = total,
                        CharacterId = characterId,
                        ItemId = itemId,
                        Equipped = false,
                        Id = Guid.NewGuid()
                    });
                }
            }

            return output.ToArray();
        }

        private static InventoryItem Map(RavenNest.Models.InventoryItem x)
        {
            var item = DataMapper.Map<InventoryItem, RavenNest.Models.InventoryItem>(x);
            item.Id = Guid.NewGuid();
            return item;
        }

        internal static User GetUser(Player player)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                PasswordHash = player.PasswordHash,
                Created = DateTime.UtcNow,
                DisplayName = player.Name,
                IsAdmin = player.IsAdmin,
                IsModerator = player.IsModerator,
                UserId = player.UserId,
                UserName = player.UserName
            };
        }

        internal static Statistics GetStatistics(Player player)
        {
            var stats = DataMapper.Map<Statistics, RavenNest.Models.Statistics>(player.Statistics);
            stats.Id = Guid.NewGuid();
            return stats;
        }

        internal static MarketItem GetMarketItem(RavenNest.Models.MarketItem item)
        {
            return new MarketItem
            {
                Id = Guid.NewGuid(),
                Amount = item.Amount,
                Created = DateTime.UtcNow,
                ItemId = item.ItemId,
                PricePerItem = item.PricePerItem
            };
        }
    }

    public class QueryBuilder
    {
        private readonly Type[] numericTypes = new Type[] {
            typeof(byte), typeof(sbyte), typeof(ushort), typeof(short), typeof(uint), typeof(int), typeof(ulong), typeof(long), typeof(decimal), typeof(float), typeof(double),
            typeof(byte?), typeof(sbyte?), typeof(ushort?), typeof(short?), typeof(uint?), typeof(int?), typeof(ulong?), typeof(long?), typeof(decimal?), typeof(float?), typeof(double?)
        };
        private readonly ConcurrentDictionary<Type, PropertyInfo[]> propertyCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<string> GetSqlReadyPropertyValues(IEntity entity, PropertyInfo[] properties)
        {
            return properties.Select(x => GetSqlReadyPropertyValue(x.PropertyType, x.GetValue(entity)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<string> GetSqlReadyPropertySet(IEntity entity, PropertyInfo[] properties)
        {
            return properties.Select(x => x.Name + "=" + GetSqlReadyPropertyValue(x.PropertyType, x.GetValue(entity)));
        }

        private string GetSqlReadyPropertyValue(Type type, object value)
        {
            if (value == null) return "NULL";
            if (type == typeof(string) || type == typeof(char)
                || type == typeof(DateTime) || type == typeof(TimeSpan)
                || type == typeof(DateTime?) || type == typeof(TimeSpan?)
                || type == typeof(Guid?) || type == typeof(Guid))
            {

                return $"'{Sanitize(value?.ToString())}'";
            }

            if (type.IsEnum)
            {
                return ((int)value).ToString();
            }

            if (numericTypes.Any(x => x == type))
            {
                return value.ToString().Replace(',', '.').Replace('−', '-');
            }

            if (typeof(bool) == type)
            {
                return (bool)value == true ? "1" : "0";
            }

            if (typeof(bool?) == type)
            {
                var b = (value as bool?);
                return b.GetValueOrDefault() ? "1" : "0";
            }

            return "NULL";
        }

        private string Sanitize(string value)
        {
            // TODO: Implement
            //       We should be using sqlparameters but how do we bulk that properly?
            return value?.Replace("'", "''");
        }
        private PropertyInfo[] GetProperties(Type type)
        {
            if (propertyCache.TryGetValue(type, out var properties))
            {
                return properties;
            }

            return propertyCache[type] = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }

        private PropertyInfo GetProperty(Type type, string propertyName)
        {
            return GetProperties(type).FirstOrDefault(x => x.Name == propertyName);
        }
        internal string Insert<T>(T entity) where T : IEntity
        {
            var type = entity.GetType();
            var props = GetProperties(type);
            var propertyNames = string.Join(", ", props.Select(x => x.Name));
            var values = string.Join(",", GetSqlReadyPropertyValues(entity, props));

            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO [" + type.Name + "]");
            sb.AppendLine("(" + propertyNames + ")");
            sb.AppendLine("VALUES (" + values + ");");
            return sb.ToString();
        }

        internal string InsertMany<T>(IEnumerable<T> entity) where T : IEntity
        {
            var type = typeof(T);
            var props = GetProperties(type);
            var propertyNames = string.Join(", ", props.Select(x => x.Name));
            var values = string.Join(",\r\n", entity.Select(x => "(" + string.Join(",", GetSqlReadyPropertyValues(x, props)) + ")"));

            var sb = new StringBuilder();
            sb.Append("INSERT INTO [" + type.Name + "] ");
            sb.AppendLine("(" + propertyNames + ") VALUES");
            sb.AppendLine(values);
            return sb.ToString();
        }

        internal string InsertIfNotExists<T>(T entity, string keyName = "Id") where T : IEntity
        {
            var type = entity.GetType();
            var props = GetProperties(type);
            var propertyNames = string.Join(", ", props.Select(x => x.Name));
            var values = string.Join(",", GetSqlReadyPropertyValues(entity, props));
            var propertySets = string.Join(",", GetSqlReadyPropertySet(entity, props.Where(x => x.Name != "Id").ToArray()));

            var sb = new StringBuilder();
            sb.AppendLine("MERGE " + type.Name + " x");
            sb.AppendLine("USING (" + propertySets + ") temp");
            sb.AppendLine("ON temp." + keyName + " = x." + keyName);
            sb.AppendLine("WHEN NOT MATCHED THEN");
            sb.AppendLine("INSERT (" + propertyNames + ")");
            sb.AppendLine("VALUES (" + values + ");");
            return sb.ToString();

            /*         
                MERGE User u
                USING (Id = user.Id, UserId = user.UserId, DisplayName = user.DisplayName, IsAdmin = user.IsAdmin, IsModerator = user.IsModerator, Created = user.Created) temp
                ON temp.UserId = u.UserId
                WHEN NOT MATCHED THEN
                INSERT (Id, UserId, DisplayName, IsAdmin, IsModerator, Created) VALUES(temp.Id, temp.UserId, temp.DisplayName, temp.IsAdmin, temp.IsModerator, temp.Created);
             */
        }
    }
}
