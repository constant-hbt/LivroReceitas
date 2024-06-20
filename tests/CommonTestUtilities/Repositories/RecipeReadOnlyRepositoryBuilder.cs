using Moq;
using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories;
public class RecipeReadOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeReadOnlyRepository> _repository;

    public RecipeReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IRecipeReadOnlyRepository>();
    }

    public RecipeReadOnlyRepositoryBuilder Filter(RecipeBook.Domain.Entities.User user, IList<RecipeBook.Domain.Entities.Recipe> recipes)
    {
        _repository.Setup(repository => repository.Filter(user, It.IsAny<FilterRecipesDto>())).ReturnsAsync(recipes);
        return this;
    }

    public IRecipeReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}
