using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI_API;
using RecipeBook.Domain.Enums;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.Cryptography;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Domain.Services.OpenAI;
using RecipeBook.Domain.Services.ServiceBus;
using RecipeBook.Domain.Services.Storage;
using RecipeBook.Infrastructure.DataAccess;
using RecipeBook.Infrastructure.DataAccess.Repositories;
using RecipeBook.Infrastructure.Extensions;
using RecipeBook.Infrastructure.Security.Cryptography;
using RecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using RecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using RecipeBook.Infrastructure.Services.LoggedUser;
using RecipeBook.Infrastructure.Services.OpenAI;
using RecipeBook.Infrastructure.Services.ServiceBus;
using RecipeBook.Infrastructure.Services.Storage;
using System.Reflection;

namespace RecipeBook.Infrastructure;
public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddPasswordEncripter(services);
        AddRepositories(services);
        AddLoggedUser(services);
        AddTokens(services, configuration);
        AddOpenAI(services, configuration);
        AddAzureStorage(services, configuration);
        AddQueue(services, configuration);

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

    private static void AddDbContext_PostgreSQL(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddDbContext<RecipeBookDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }

    private static void AddDbContext_MySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        services.AddDbContext<RecipeBookDbContext>(options =>
        {
            options.UseMySql(connectionString, serverVersion);
        });
    }

    private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
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
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUserDeleteOnlyRepository, UserRepository>();
        services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();
    }

    private static void AddFluentMigrator_PostgreSQL(IServiceCollection services, IConfiguration configuration)
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

    private static void AddFluentMigrator_MySql(IServiceCollection services, IConfiguration configuration)
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

    private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration)
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

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey")!;

        services.AddScoped<IAccessTokenGenerator>(options => new JwtTokenGenerator(expirationTimeMinutes, signingKey));
        services.AddScoped<IAccessTokenValidator>(options => new JwtTokenValidator(signingKey));
    }

    private static void AddLoggedUser(IServiceCollection services)
    {
        services.AddScoped<ILoggedUser, LoggedUser>();
    }

    private static void AddPasswordEncripter(IServiceCollection services)
    {
        // Substituindo o antigo método de encriptar as senhas pelo BCrypt
        //var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey")!;
        //services.AddScoped<IPasswordEncripter>(options => new Sha512Encripter(additionalKey));

        services.AddScoped<IPasswordEncripter, BCryptNet>();
    }

    private static void AddOpenAI(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGenerateRecipeAI, ChatGPTService>();

        var apiKey = configuration.GetValue<string>("Settings:OpenAI:ApiKey")!;
        var authentication = new APIAuthentication(apiKey);

        services.AddScoped<IOpenAIAPI>(option => new OpenAIAPI(authentication));
    }

    private static void AddAzureStorage(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGenerateRecipeAI, ChatGPTService>();

        var connectionString = configuration.GetValue<string>("Settings:BlobStorage:Azure");

        if (connectionString.NotEmpty())
            services.AddScoped<IBlobStorageService>(option => new AzureStorageService(new BlobServiceClient(connectionString!)));
    }

    private static void AddQueue(IServiceCollection services, IConfiguration configuration)
    {
        const string QUEUE_NAME = "user"; // User -> Nome da fila no Azure
        var connectionString = configuration.GetValue<string>("Settings:ServiceBus:DeleteUserAccount")!;

        if (string.IsNullOrWhiteSpace(connectionString))
            return;

        var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        });

        // Criando o ServiceBusProcessor e registrando-o como singleton
        services.AddSingleton(sp =>
        {
            return client.CreateProcessor(QUEUE_NAME, new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1
            });
        });

        // Registrando DeleteUserQueue
        services.AddScoped<IDeleteUserQueue, DeleteUserQueue>(sp =>
        {
            return new DeleteUserQueue(client.CreateSender(QUEUE_NAME));
        });
    }
}
