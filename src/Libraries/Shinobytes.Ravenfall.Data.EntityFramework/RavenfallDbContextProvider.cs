using Microsoft.Data.SqlClient;
using Shinobytes.Ravenfall.RavenNet.Core;

namespace Shinobytes.Ravenfall.Data.EntityFramework.Legacy
{
    public class RavenfallDbContextProvider : IRavenfallDbContextProvider
    {
        private readonly IAppSettings settings;

        public RavenfallDbContextProvider(IAppSettings settings)
        {
            this.settings = settings;
        }

        public RavenfallDbContext Get()
        {
            var ctx = new RavenfallDbContext(settings.DbConnectionString);
            ctx.ChangeTracker.AutoDetectChangesEnabled = false;
            return ctx;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(settings.DbConnectionString);
        }
    }
}