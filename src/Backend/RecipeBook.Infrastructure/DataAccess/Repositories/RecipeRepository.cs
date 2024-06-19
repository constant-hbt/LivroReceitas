using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Recipe;

namespace RecipeBook.Infrastructure.DataAccess.Repositories;
public class RecipeRepository(RecipeBookDbContext dbContext) : IRecipeWriteOnlyRepository
{
    private readonly RecipeBookDbContext _dbContext = dbContext;

    public async Task Add(Recipe recipe)
    {
        await _dbContext.Recipes.AddAsync(recipe);
    }
}
