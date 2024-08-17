using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using RecipeBook.Application.UseCases.Recipe;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Entities;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using UseCases.Test.Recipe.InlineDatas;
using Xunit;

namespace UseCases.Test.Recipe.Register;
public class RegisterRecipeUseCaseTest
{
    [Fact]
    public async Task Success_Without_Image()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestRegisterRecipeFormDataBuilder.Build();
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Id.Should().NotBeNullOrWhiteSpace();
        result.Title.Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(ImageTypesInlineData))]
    public async Task Success_With_Image(IFormFile file)
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestRegisterRecipeFormDataBuilder.Build(file);
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Id.Should().NotBeNullOrWhiteSpace();
        result.Title.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestRegisterRecipeFormDataBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task<ResponseRegisteredRecipeJson>> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesExceptions.RECIPE_TITLE_EMPTY));
    }

    [Fact]
    public async Task Error_Invalid_File()
    {
        var (user, _) = UserBuilder.Build();

        var textFile = FormFileBuilder.Txt();

        var request = RequestRegisterRecipeFormDataBuilder.Build(textFile);

        var useCase = CreateUseCase(user);

        Func<Task<ResponseRegisteredRecipeJson>> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesExceptions.ONLY_IMAGES_ACCEPTED));
    }

    private static RegisterRecipeUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = RecipeWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var blobStorage = new BlobStorageServiceBuilder().Build();

        return new RegisterRecipeUseCase(loggedUser, repository, mapper, unitOfWork, blobStorage);
    }
}
