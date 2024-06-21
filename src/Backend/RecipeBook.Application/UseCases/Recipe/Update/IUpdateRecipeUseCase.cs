using RecipeBook.Communication.Requests;

namespace RecipeBook.Application.UseCases.Recipe.Update;
public interface IUpdateRecipeUseCase
{
    public Task Execute(long recipeId, RequestRecipeJson request);
}
