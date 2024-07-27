using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using Microsoft.AspNetCore.Http;
using RecipeBook.Application.Extensions;
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

        var (isValidImage, extension) = fileStream.ValidateAndGetImageExtension();

        if (isValidImage.IsFalse())
            throw new ErrorOnValidationException([ResourceMessagesExceptions.ONLY_IMAGES_ACCEPTED]);

        if (recipe.ImageIdentifier.IsEmpty())
        {
            recipe.ImageIdentifier = $"{Guid.NewGuid()}{extension}";

            _repository.Update(recipe);

            await _unitOfWork.Commit();
        }

        await _blobStorageService.Upload(loggedUser, fileStream, recipe.ImageIdentifier!);
    }
}
