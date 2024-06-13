using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Infrastructure.DataAccess.Converters;

namespace RecipeBook.Infrastructure.DataAccess;

public class RecipeBookDbContext : DbContext
{
    public RecipeBookDbContext(DbContextOptions<RecipeBookDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeBookDbContext).Assembly);

        var dateTimeConverter = new UtcDateTimeConverter();
        var nullableDateTimeConverter = new NullableUtcDateTimeConverter();

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                    property.SetValueConverter(dateTimeConverter);
                else if (property.ClrType == typeof(DateTime?))
                    property.SetValueConverter(nullableDateTimeConverter);
            }
        }
    }
}
