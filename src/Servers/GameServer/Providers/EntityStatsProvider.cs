
using RavenNest.BusinessLogic.Data;
using Shinobytes.Ravenfall.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RavenfallServer.Providers
{
    public abstract class EntityStatsProvider : IEntityStatsProvider
    {
        private Dictionary<string, PropertyInfo> attributesProps = new Dictionary<string, PropertyInfo>();
        private Dictionary<string, PropertyInfo> professionsProps = new Dictionary<string, PropertyInfo>();

        private readonly IGameData gameData;
        private readonly bool isPlayer;

        public EntityStatsProvider(IGameData gameData, bool isPlayer)
        {
            this.gameData = gameData;
            this.isPlayer = isPlayer;
        }

        public int AddExperience(int entityId, string skill, decimal amount)
        {
            var experience = GetExperience(entityId, skill);
            var level = GetLevel(entityId, skill);
            var delta = GameMath.ExperienceToLevel(experience) - level;

            experience += amount;
            level += delta;

            SetExperience(entityId, skill, experience);
            SetLevel(entityId, skill, level);

            return delta;
        }

        public decimal GetExperience(int entityId, string name)
        {
            return GetValue<decimal>(entityId, name + "exp");
        }

        public int GetLevel(int entityId, string name)
        {
            return GetValue<int>(entityId, name);
        }

        public void SetLevel(int entityId, string skill, int level)
        {
            SetValue(entityId, skill, level);
        }

        public void SetExperience(int entityId, string skill, decimal experience)
        {
            SetValue(entityId, skill + "exp", experience);
        }

        private void SetValue<T>(int entityId, string skill, T value)
        {
            var attributesId = isPlayer ? gameData.GetPlayer(entityId).AttributesId : gameData.GetNpc(entityId).AttributesId;
            var attributes = gameData.GetAttributes(attributesId);

            var attr = GetAttributeProperty(skill);
            if (attr != null)
            {
                attr.SetValue(attributes, value);
                return;
            }

            if (isPlayer)
            {
                var professions = gameData.GetProfessions(gameData.GetPlayer(entityId).ProfessionsId);
                var prof = GetProfessionProperty(skill);
                if (prof != null)
                {
                    prof.SetValue(professions, value);
                }
            }
        }

        private T GetValue<T>(int entityId, string name)
        {
            var attributesId = isPlayer ? gameData.GetPlayer(entityId).AttributesId : gameData.GetNpc(entityId).AttributesId;
            var attributes = gameData.GetAttributes(attributesId);

            var attr = GetAttributeProperty(name);
            if (attr != null)
            {
                return (T)attr.GetValue(attributes);
            }

            if (isPlayer)
            {
                var professions = gameData.GetProfessions(gameData.GetPlayer(entityId).ProfessionsId);
                var prof = GetProfessionProperty(name);
                if (prof != null)
                {
                    return (T)prof.GetValue(professions);
                }
            }

            return default;
        }

        private PropertyInfo GetProfessionProperty(string name)
        {
            if (professionsProps.Count == 0)
            {
                professionsProps = typeof(Professions).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(x => x.Name.ToLower(), y => y);
            }
            if (professionsProps.TryGetValue(name.ToLower(), out var prop))
            {
                return prop;
            }
            return null;
        }

        private PropertyInfo GetAttributeProperty(string name)
        {
            if (attributesProps.Count == 0)
            {
                attributesProps = typeof(Attributes).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(x => x.Name.ToLower(), y => y);
            }
            if (attributesProps.TryGetValue(name.ToLower(), out var attr))
            {
                return attr;
            }
            return null;
        }

    }
}