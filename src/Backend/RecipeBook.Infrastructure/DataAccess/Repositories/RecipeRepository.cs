using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Repositories.Recipe;

namespace RecipeBook.Infrastructure.DataAccess.Repositories;
public class RecipeRepository(RecipeBookDbContext dbContext) : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository, IRecipeUpdateOnlyRepository
{
    private readonly RecipeBookDbContext _dbContext = dbContext;

    public async Task Add(Recipe recipe)
    {
        await _dbContext.Recipes.AddAsync(recipe);
    }

    public async Task Delete(long recipeId)
    {
        var recipe = await _dbContext.Recipes.FindAsync(recipeId);

        _dbContext.Recipes.Remove(recipe!);
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

    async Task<Recipe?> IRecipeReadOnlyRepository.GetById(User user, long recipeId)
    {
        return await GetFullRecipe()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(recipe => recipe.Active && recipe.Id == recipeId && recipe.UserId == user.Id);
    }

    async Task<Recipe?> IRecipeUpdateOnlyRepository.GetById(User user, long recipeId)
    {
        return await GetFullRecipe()
                        .FirstOrDefaultAsync(recipe => recipe.Active && recipe.Id == recipeId && recipe.UserId == user.Id);
    }

    public void Update(Recipe recipe)
    {
        _dbContext.Recipes.Update(recipe);
    }

    private IIncludableQueryable<Recipe, IList<DishType>> GetFullRecipe()
    {
        return _dbContext.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Instructions)
            .Include(r => r.DishTypes);
    }

    public async Task<IList<Recipe>> GetForDashboard(User user)
    {
        return await _dbContext.Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
            .Where(r => r.Active && r.UserId == user.Id)
            .OrderByDescending(r => r.CreatedOn)
            .Take(5)
            .ToListAsync();
    }
}
