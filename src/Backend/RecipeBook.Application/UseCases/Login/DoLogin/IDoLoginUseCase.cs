using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UseCases.Login.DoLogin;
public interface IDoLoginUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
