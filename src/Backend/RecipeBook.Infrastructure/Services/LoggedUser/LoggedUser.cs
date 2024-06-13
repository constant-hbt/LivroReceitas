using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Security.Tokens;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Infrastructure.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RecipeBook.Infrastructure.Services.LoggedUser;
public class LoggedUser(RecipeBookDbContext dbContext, ITokenProvider tokenProvider) : ILoggedUser
{
    private readonly RecipeBookDbContext _dbContext = dbContext;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<User> User()
    {
        var token = _tokenProvider.Value();
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        var userIdentifier = Guid.Parse(identifier);

        return await _dbContext.Users.AsNoTracking().FirstAsync(user => user.Active && user.UserIdentifier == userIdentifier);
    }
}
