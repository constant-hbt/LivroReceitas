using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;

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
    }
}
