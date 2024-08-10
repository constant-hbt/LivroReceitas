using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using RecipeBook.Application.UseCases.Login.DoLogin;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Xunit;

namespace UseCases.Test.Login.DoLogin;
public class DoLoginUseCaseTest
{
    private static DoLoginUseCase CreateUseCase(RecipeBook.Domain.Entities.User? user = null)
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        if (user is not null)
            userReadOnlyRepositoryBuilder.GetByEmail(user);

        var userReadOnlyRepository = userReadOnlyRepositoryBuilder.Build();

        return new DoLoginUseCase(userReadOnlyRepository, passwordEncripter, accessTokenGenerator);
    }

    [Fact]
    public async Task Success()
    {
        var (user, password) = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(new RequestLoginJson
        {
            Email = user.Email,
            Password = password,
        });

        result.Should().NotBeNull();
        result.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
        result.Tokens.Should().NotBeNull();
        result.Tokens.AccessToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Invalid_user()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<InvalidLoginException>()
            .Where(e => e.Message.Equals(ResourceMessagesExceptions.EMAIL_OR_PASSWORD_INVALID));
    }
}
