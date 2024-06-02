using Moq;
using RecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;
public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        return new Mock<IUserWriteOnlyRepository>().Object;
    }
}
