namespace RecipeBook.Domain.Security.Tokens;
public interface IRefreshTokenGenerator
{
    string Generate();
}
