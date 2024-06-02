using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Enums;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Infrastructure.DataAccess;
using RecipeBook.Infrastructure.DataAccess.Repositories;
using RecipeBook.Infrastructure.Extensions;
using System.Reflection;

namespace RecipeBook.Infrastructure;
public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfigurationManager configuration)
    {
        AddRepositories(services);

        if (configuration.IsUnitTestEnviroment())
            return;

        var databaseType = configuration.DatabaseType();

        if (databaseType == DatabaseType.PostgreSQL)
        {
            AddDbContext_PostgreSQL(services, configuration);
            AddFluentMigrator_PostgreSQL(services, configuration);
        }
        else if (databaseType == DatabaseType.MySql)
        {
            AddDbContext_MySql(services, configuration);
            AddFluentMigrator_MySql(services, configuration);
        } 
        else if (databaseType == DatabaseType.SqlServer)
        {
            AddDbContext_SqlServer(services, configuration);
            AddFluentMigrator_SqlServer(services, configuration);
        }  
        else
            throw new NotImplementedException();
    }

    private static void AddDbContext_PostgreSQL(IServiceCollection services, IConfigurationManager configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddDbContext<RecipeBookDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }

    private static void AddDbContext_MySql(IServiceCollection services, IConfigurationManager configuration)
    {
        var connectionString = configuration.ConnectionString();
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        services.AddDbContext<RecipeBookDbContext>(options =>
        {
            options.UseMySql(connectionString, serverVersion);
        });
    }

    private static void AddDbContext_SqlServer(IServiceCollection services, IConfigurationManager configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddDbContext<RecipeBookDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }

    private static void AddRepositories(IServiceCollection services) 
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
    }

    private static void AddFluentMigrator_PostgreSQL(IServiceCollection services, IConfigurationManager configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("RecipeBook.Infrastructure")).For.All();
        });
    }

    private static void AddFluentMigrator_MySql(IServiceCollection services, IConfigurationManager configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddMySql8()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("RecipeBook.Infrastructure")).For.All();
        });
    }

    private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfigurationManager configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("RecipeBook.Infrastructure")).For.All();
        });
    }
}
