
using AutoMapper;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Services.LoggedUser;

namespace RecipeBook.Application.UseCases.User.Profile;
public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.User();

        return _mapper.Map<ResponseUserProfileJson>(user);
    }
}
