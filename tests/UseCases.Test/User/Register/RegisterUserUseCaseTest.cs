using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Extensions;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using Xunit;

namespace UseCases.Test.User.Register;
public class RegisterUserUseCaseTest
{
    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        if (email.NotEmpty())
            readRepositoryBuilder.ExistActiveUserWithEmail(email);

        var readRepository = readRepositoryBuilder.Build();

        return new RegisterUserUseCase(writeRepository, readRepository, mapper, passwordEncripter, unitOfWork, accessTokenGenerator);
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestRegisterUserJsonBuilder().Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Tokens.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Tokens.AccessToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = new RequestRegisterUserJsonBuilder().Build();
        var useCase = CreateUseCase(request.Email);

        Func<Task<ResponseRegisteredUserJson>> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesExceptions.EMAIL_ALREADY_REGISTERED));
    }
}
