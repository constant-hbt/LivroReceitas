using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.User;

namespace RecipeBook.Infrastructure.DataAccess.Repositories;

public class UserRepository(RecipeBookDbContext dbContext) : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly RecipeBookDbContext _dbContext = dbContext;

    public async Task Add(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        email = email.ToLower();
        return await _dbContext.Users.AnyAsync(user => user.Active && user.Email.ToLower().Equals(email));
    }

    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email) && user.Password.Equals(password));
    }

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier)
    {
        return await _dbContext.Users.AnyAsync(user => user.Active && user.UserIdentifier.Equals(userIdentifier));
    }

    public async Task<User> GetByUserIdentifier(Guid userIdentifier)
    {
        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.Active && user.UserIdentifier.Equals(userIdentifier));
    }

    public async Task<User> GetById(long id)
    {
        return await _dbContext.Users.FirstAsync(user => user.Id == id);
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }
}
