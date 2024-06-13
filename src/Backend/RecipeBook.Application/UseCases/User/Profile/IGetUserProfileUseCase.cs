using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UseCases.User.Profile;
public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute();
}
