using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Npgsql;
using RecipeBook.Domain.Enums;
using System.Net;

namespace RecipeBook.Infrastructure.Migrations;
public static class DatabaseMigration
{
    public static void Migrate(DatabaseType databaseType, string connectionString, IServiceProvider serviceProvider)
    {
        if (databaseType == DatabaseType.PostgreSQL)
            EnsureDatabaseCreated_PostgreSQL(connectionString);
        else if (databaseType == DatabaseType.MySql)
            EnsureDatabaseCreated_MySQL(connectionString);
        else if (databaseType == DatabaseType.SqlServer)
            EnsureDatabaseCreated_SqlServer(connectionString);
        else
            throw new NotImplementedException();

        MigrationDatabase(serviceProvider);
    }

    private static void EnsureDatabaseCreated_PostgreSQL(string connectionString)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
        var databaseName = connectionStringBuilder.Database;

        // Removendo a informação do Database para se conectar ao servidor, 
        // ao invés do banco de dados específico para conseguir criar o banco de dados pretendido
        connectionStringBuilder.Remove("Database");

        using var dbConnection = new NpgsqlConnection(connectionStringBuilder.ConnectionString);

        dbConnection.Open();

        // Verificar se o banco de dados já existe
        var databaseExists = dbConnection.QueryFirstOrDefault<bool>(
            "SELECT EXISTS(SELECT 1 FROM pg_database WHERE datname = @databaseName);",
            new { databaseName }
        );

        if (!databaseExists)
            dbConnection.Execute($"CREATE DATABASE \"{databaseName}\";");
    }

    private static void EnsureDatabaseCreated_MySQL(string connectionString)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);
        var databaseName = connectionStringBuilder.Database;

        // Removendo a informação do Database para se conectar ao servidor, 
        // ao invés do banco de dados específico para conseguir criar o banco de dados pretendido
        connectionStringBuilder.Remove("Database");

        using var dbConnection = new MySqlConnection(connectionStringBuilder.ConnectionString);

        dbConnection.Open();

        dbConnection.Execute($"CREATE DATABASE IF NOT EXISTS `{databaseName}`;", new { databaseName });
    }

    private static void EnsureDatabaseCreated_SqlServer(string connectionString)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        var databaseName = connectionStringBuilder.InitialCatalog;

        // Removendo a informação do Database para se conectar ao servidor, 
        // ao invés do banco de dados específico para conseguir criar o banco de dados pretendido
        connectionStringBuilder.Remove("Database");

        using var dbConnection = new SqlConnection(connectionStringBuilder.ConnectionString);

        dbConnection.Open();

        var records = dbConnection.Query("SELECT * FROM sys.databases WHERE name = @databaseName", new { databaseName });

        if (!records.Any())
            dbConnection.Execute($"CREATE DATABASE [{databaseName}];");
    }

    private static void MigrationDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.ListMigrations();
        runner.MigrateUp();
    }
}
