using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using Microsoft.AspNetCore.Http;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Domain.Services.Storage;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Recipe.Image;
public class AddUpdateImageCoverUseCase : IAddUpdateImageCoverUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public AddUpdateImageCoverUseCase(
        ILoggedUser loggedUser,
        IRecipeUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IBlobStorageService blobStorageService)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    public async Task Execute(long recipeId, IFormFile file)
    {
        var loggedUser = await _loggedUser.User();

        var recipe = await _repository.GetById(loggedUser, recipeId) 
            ?? throw new NotFoundException(ResourceMessagesExceptions.RECIPE_NOT_FOUND);

        var fileStream = file.OpenReadStream();

        if (fileStream.Is<PortableNetworkGraphic>().IsFalse() && fileStream.Is<JointPhotographicExpertsGroup>().IsFalse())
            throw new ErrorOnValidationException([ResourceMessagesExceptions.ONLY_IMAGES_ACCEPTED]);

        if (recipe.ImageIdentifier.IsEmpty())
        {
            recipe.ImageIdentifier = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            _repository.Update(recipe);

            await _unitOfWork.Commit();
        }

        // Permitindo que a stream seja lida novamente
        fileStream.Position = 0;

        await _blobStorageService.Upload(loggedUser, fileStream, recipe.ImageIdentifier!);
    }
}
