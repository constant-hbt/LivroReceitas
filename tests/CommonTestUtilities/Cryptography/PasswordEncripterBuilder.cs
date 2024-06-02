using RecipeBook.Application.Services.Cryptography;

namespace CommonTestUtilities.Cryptography;
public class PasswordEncripterBuilder
{
    public static PasswordEncripter Build()
    {
        return new PasswordEncripter("abc1234");
    }
}
