using Microsoft.Extensions.Configuration;
using RecipeBook.Domain.Enums;

namespace RecipeBook.Infrastructure.Extensions;
public static class ConfigurationExtension
{
    public static DatabaseType DatabaseType(this IConfiguration configuration)
    {
        var databaseType = configuration.GetConnectionString("DatabaseType")!;
        return (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType);
    }

    public static string ConnectionString(this IConfiguration configuration)
    {
        var databaseType = configuration.DatabaseType();

        if (databaseType == Domain.Enums.DatabaseType.PostgreSQL)
            return configuration.GetConnectionString("ConnectionPostgreSQL")!;
        else if (databaseType == Domain.Enums.DatabaseType.MySql)
            return configuration.GetConnectionString("ConnectionMySql")!;
        else if (databaseType == Domain.Enums.DatabaseType.SqlServer)
            return configuration.GetConnectionString("ConnectionSqlServer")!;
        
        throw new NotImplementedException();
    }
}
