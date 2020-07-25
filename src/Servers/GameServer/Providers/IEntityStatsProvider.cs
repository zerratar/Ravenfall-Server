namespace RavenfallServer.Providers
{
    public interface IEntityStatsProvider
    {
        decimal GetExperience(int entityId, string name);
        int GetLevel(int entityId, string name);
        int AddExperience(int entityId, string skill, decimal experience);
        void SetLevel(int entityId, string skill, int level);
        void SetExperience(int entityId, string skill, decimal experience);
    }
}