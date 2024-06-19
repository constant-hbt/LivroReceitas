using Moq;
using RecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories;
public class RecipeWriteOnlyRepositoryBuilder
{
    public static IRecipeWriteOnlyRepository Build()
    {
        return new Mock<IRecipeWriteOnlyRepository>().Object;
    }
}
