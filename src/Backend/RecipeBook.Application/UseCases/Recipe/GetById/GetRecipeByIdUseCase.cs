using AutoMapper;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Domain.Services.Storage;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Recipe.GetById;
public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IBlobStorageService _blobStorageService;

    public GetRecipeByIdUseCase(IMapper mapper,
        ILoggedUser loggedUser,
        IRecipeReadOnlyRepository repository,
        IBlobStorageService blobStorageService)
    {
        _mapper = mapper;
        _loggedUser = loggedUser;
        _repository = repository;
        _blobStorageService = blobStorageService;
    }

    public async Task<ResponseRecipeJson> Execute(long recipeId)
    {
        var loggedUser = await _loggedUser.User();

        var recipe = await _repository.GetById(loggedUser, recipeId) 
            ?? throw new NotFoundException(ResourceMessagesExceptions.RECIPE_NOT_FOUND);

        var response = _mapper.Map<ResponseRecipeJson>(recipe);

        if (recipe.ImageIdentifier.NotEmpty())
        {
            var url = await _blobStorageService.GetFileUrl(loggedUser, recipe.ImageIdentifier!);

            response.ImageUrl = url;
        }

        return response;
    }
}
