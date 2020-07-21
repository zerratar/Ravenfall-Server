using Microsoft.Data.SqlClient;

namespace Shinobytes.Ravenfall.Data.EntityFramework.Legacy
{
    public interface IRavenfallDbContextProvider
    {
        SqlConnection GetConnection();
        RavenfallDbContext Get();
    }
}