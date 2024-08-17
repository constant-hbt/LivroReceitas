using Microsoft.AspNetCore.Http;

namespace RecipeBook.Application.UseCases.Recipe.Image;
public interface IAddUpdateImageCoverUseCase
{
    Task Execute(long recipeId, IFormFile file);
}
