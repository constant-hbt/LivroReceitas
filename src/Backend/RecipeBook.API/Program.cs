using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using RecipeBook.API.BackgroundServices;
using RecipeBook.API.Converters;
using RecipeBook.API.Filters;
using RecipeBook.API.Middleware;
using RecipeBook.API.Token;
using RecipeBook.Application;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Infrastructure;
using RecipeBook.Infrastructure.DataAccess;
using RecipeBook.Infrastructure.Extensions;
using RecipeBook.Infrastructure.Migrations;

const string AUTHENTICATION_TYPE = "Bearer";

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new StringConverter());
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    options.JsonSerializerOptions.Converters.Add(new NullableDateTimeConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<IdsFilter>();

    options.AddSecurityDefinition(AUTHENTICATION_TYPE, new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                        Enter 'Bearer' [space] and then your token in the next input below.
                        Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = AUTHENTICATION_TYPE
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = AUTHENTICATION_TYPE
                },
                Scheme = "oauth2",
                Name = AUTHENTICATION_TYPE,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddInfrastructure(configuration);
builder.Services.AddApplication(configuration);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Permite a injeção de dependências do HttpContextAccessor, utilizado para resgatar o userIdentifier contido no token JWT das requsições
builder.Services.AddHttpContextAccessor();

if (builder.Configuration.IsUnitTestEnviroment().IsFalse())
{
    builder.Services.AddHostedService<DeleteUserService>();

    AddGoogleAuthentication();
}

builder.Services.AddHealthChecks().AddDbContextCheck<RecipeBookDbContext>();

var app = builder.Build();

app.MapHealthChecks("/Health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

await app.RunAsync();

void MigrateDatabase()
{
    if (builder.Configuration.IsUnitTestEnviroment())
        return;

    var databaseType = configuration!.DatabaseType();
    var connectionString = configuration!.ConnectionString();

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DatabaseMigration.Migrate(databaseType, connectionString, serviceScope.ServiceProvider);
}

void AddGoogleAuthentication()
{
    var clientId = builder.Configuration.GetValue<string>("Settings:Google:ClientId")!;
    var clientSecret = builder.Configuration.GetValue<string>("Settings:Google:ClientSecret")!;

    builder.Services.AddAuthentication(config =>
    {
        config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    }).AddCookie()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = clientId;
        googleOptions.ClientSecret = clientSecret;
    });
}

// Necessário para ser utilizado em testes de integração, no projeto WebApi.Test
public partial class Program 
{
    protected Program() { }
}