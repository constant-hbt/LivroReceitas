using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Token;

namespace RecipeBook.Infrastructure.DataAccess.Repositories;
public class TokenRepository(RecipeBookDbContext dbContext) : ITokenRepository
{
    private readonly RecipeBookDbContext _dbContext = dbContext;

    public async Task<RefreshToken?> Get(string refreshToken)
    {
        return await _dbContext
            .RefreshTokens
            .AsNoTracking()
            .Include(token => token.User)
            .FirstOrDefaultAsync(token => token.Value.Equals(refreshToken));
    }

    public async Task SaveNewRefreshToken(RefreshToken refreshToken)
    {
        var tokens = _dbContext.RefreshTokens.Where(token => token.UserId == refreshToken.UserId);

        _dbContext.RefreshTokens.RemoveRange(tokens);

        await _dbContext.RefreshTokens.AddAsync(refreshToken);
    }
}
