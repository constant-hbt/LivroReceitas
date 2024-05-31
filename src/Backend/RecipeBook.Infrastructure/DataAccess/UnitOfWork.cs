using RecipeBook.Domain.Repositories;

namespace RecipeBook.Infrastructure.DataAccess;
public class UnitOfWork(RecipeBookDbContext dbContext) : IUnitOfWork
{
    private readonly RecipeBookDbContext _dbContext = dbContext;

    public async Task<int> Commit()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
