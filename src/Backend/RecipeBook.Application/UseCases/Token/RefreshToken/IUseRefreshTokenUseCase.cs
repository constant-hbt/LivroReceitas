using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UseCases.Token.RefreshToken;
public interface IUseRefreshTokenUseCase
{
    Task<ResponseTokensJson> Execute(RequestNewTokenJson request);
}
