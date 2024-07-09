using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UseCases.Recipe.Generate;
public interface IGenerateRecipeUseCase
{
    Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request);
}
