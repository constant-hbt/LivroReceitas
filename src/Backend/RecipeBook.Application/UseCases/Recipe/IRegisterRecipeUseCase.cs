using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UseCases.Recipe;
public interface IRegisterRecipeUseCase
{
    Task<ResponseRegisteredRecipeJson> Execute(RequestRegisterRecipeFormData request);
}
