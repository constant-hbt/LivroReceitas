using AutoMapper;
using RecipeBook.Application.Extensions;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Domain.Services.Storage;

namespace RecipeBook.Application.UseCases.Dashboard;
public class GetDashboardUseCase : IGetDashboardUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IBlobStorageService _blobStorageService;

    public GetDashboardUseCase(
        ILoggedUser loggedUser,
        IMapper mapper,
        IRecipeReadOnlyRepository repository,
        IBlobStorageService blobStorageService)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
        _repository = repository;
        _blobStorageService = blobStorageService;
    }

    public async Task<ResponseRecipesJson> Execute()
    {
        var loggedUser = await _loggedUser.User();

        var recipes = await _repository.GetForDashboard(loggedUser);

        return new ResponseRecipesJson
        {
            Recipes = await recipes.MaptoShortRecipeJson(loggedUser, _blobStorageService, _mapper)
        };
    }
}
