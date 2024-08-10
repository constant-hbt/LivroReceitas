using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Repositories.Token;
public interface ITokenRepository
{
    Task<RefreshToken?> Get(string refreshToken);
    Task SaveNewRefreshToken(RefreshToken refreshToken);
}
