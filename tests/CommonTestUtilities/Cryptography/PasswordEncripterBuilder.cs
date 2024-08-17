using RecipeBook.Domain.Security.Cryptography;
using RecipeBook.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;
public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build()
    {
        return new BCryptNet();
    }
}
