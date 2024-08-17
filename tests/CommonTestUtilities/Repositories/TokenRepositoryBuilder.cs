using Moq;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Token;

namespace CommonTestUtilities.Repositories;
public class TokenRepositoryBuilder
{
    private readonly Mock<ITokenRepository> _repository;

    public TokenRepositoryBuilder()
    {
        _repository = new Mock<ITokenRepository>();
    }

    public TokenRepositoryBuilder GetById(RefreshToken refreshToken)
    {
        _repository.Setup(x => x.Get(refreshToken.Value)).ReturnsAsync(refreshToken);
        return this;
    }

    public ITokenRepository Build() => _repository.Object;
}
