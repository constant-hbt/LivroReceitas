using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.Recipe;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.Application.UseCases.Recipe.Delete;
public class DeleteRecipeUseCase : IDeleteRecipeUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _recipeReadOnlyRepository;
    private readonly IRecipeWriteOnlyRepository _recipeWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRecipeUseCase(
        ILoggedUser loggedUser,
        IRecipeReadOnlyRepository recipeReadOnlyRepository,
        IRecipeWriteOnlyRepository recipeWriteOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _recipeReadOnlyRepository = recipeReadOnlyRepository;
        _recipeWriteOnlyRepository = recipeWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long recipeId)
    {
        var loggedUser = await _loggedUser.User();

        _ = await _recipeReadOnlyRepository.GetById(loggedUser, recipeId) ?? throw new NotFoundException(ResourceMessagesExceptions.RECIPE_NOT_FOUND);

        await _recipeWriteOnlyRepository.Delete(recipeId);

        await _unitOfWork.Commit();
    }
}
