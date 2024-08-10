using RecipeBook.Domain.Security.Tokens;

namespace RecipeBook.Infrastructure.Security.Tokens.Refresh;
public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
}
