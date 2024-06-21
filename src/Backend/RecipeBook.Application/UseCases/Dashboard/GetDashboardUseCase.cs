using AutoMapper;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;

namespace RecipeBook.Application.UseCases.Dashboard;
public class GetDashboardUseCase : IGetDashboardUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;
    private readonly IRecipeReadOnlyRepository _repository;

    public GetDashboardUseCase(
        ILoggedUser loggedUser,
        IMapper mapper,
        IRecipeReadOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ResponseRecipesJson> Execute()
    {
        var loggedUser = await _loggedUser.User();

        var recipes = await _repository.GetForDashboard(loggedUser);

        return new ResponseRecipesJson
        {
            Recipes = _mapper.Map<IList<ResponseShortRecipeJson>>(recipes)
        };
    }
}
