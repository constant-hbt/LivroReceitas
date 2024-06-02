using RecipeBook.Domain.Repositories;

namespace RecipeBook.Infrastructure.DataAccess;
public class UnitOfWork(RecipeBookDbContext dbContext) : IUnitOfWork
{
    private readonly RecipeBookDbContext _dbContext = dbContext;

    public async Task Commit()
    {
        await _dbContext.SaveChangesAsync();
    }
}
