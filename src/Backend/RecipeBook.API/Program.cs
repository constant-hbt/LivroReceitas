using RecipeBook.API.Converters;
using RecipeBook.API.Filters;
using RecipeBook.API.Middleware;
using RecipeBook.Application;
using RecipeBook.Infrastructure;
using RecipeBook.Infrastructure.Extensions;
using RecipeBook.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(configuration);
builder.Services.AddApplication(configuration);

builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

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

// Necessário para ser utilizado em testes de integração, no projeto WebApi.Test
public partial class Program 
{
    protected Program() { }
}