using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Repositories.Recipe;
using System.Linq;

namespace RecipeBook.Infrastructure.DataAccess.Repositories;
public class RecipeRepository(RecipeBookDbContext dbContext) : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository
{
    private readonly RecipeBookDbContext _dbContext = dbContext;

    public async Task Add(Recipe recipe)
    {
        await _dbContext.Recipes.AddAsync(recipe);
    }

    public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filter)
    {
        var query = _dbContext.Recipes
                        .AsNoTracking()
                        .Include(recipe => recipe.Ingredients)
                        .Where(recipe => recipe.Active && recipe.UserId == user.Id);

        if (filter.Difficulties.Any())
            query = query.Where(recipe => recipe.Difficulty.HasValue && filter.Difficulties.Contains(recipe.Difficulty.Value));

        if (filter.CookingTimes.Any())
            query = query.Where(recipe => recipe.CookingTime.HasValue && filter.CookingTimes.Contains(recipe.CookingTime.Value));

        if (filter.DishTypes.Any())
            query = query.Where(recipe => recipe.DishTypes.Any(dishType => filter.DishTypes.Contains(dishType.Type)));

        if (filter.RecipeTitle_Ingredient.NotEmpty())
            query = query.Where(recipe => recipe.Title.Contains(filter.RecipeTitle_Ingredient) 
                                || recipe.Ingredients.Any(ingredient => ingredient.Item.Contains(filter.RecipeTitle_Ingredient)));

        return await query.ToListAsync();
    }
}
