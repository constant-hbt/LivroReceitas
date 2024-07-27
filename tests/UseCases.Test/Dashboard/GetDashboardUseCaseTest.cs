using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using RecipeBook.Application.UseCases.Dashboard;
using Xunit;

namespace UseCases.Test.Dashboard;
public class GetDashboardUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var recipes = RecipeBuilder.Collection(user);

        var useCase = CreateUseCase(user, recipes);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Recipes.Should()
            .HaveCountGreaterThan(0)
            .And.OnlyHaveUniqueItems(r => r.Id)
            .And.AllSatisfy(r =>
            {
                r.Id.Should().NotBeNullOrWhiteSpace();
                r.Title.Should().NotBeNullOrWhiteSpace();
                r.AmountIngredients.Should().BeGreaterThan(0);
                r.ImageUrl.Should().NotBeNullOrWhiteSpace();
            });
    }

    private static GetDashboardUseCase CreateUseCase(RecipeBook.Domain.Entities.User user, IList<RecipeBook.Domain.Entities.Recipe> recipes)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeReadOnlyRepositoryBuilder().GetForDashboard(user, recipes).Build();
        var blobStorage = new BlobStorageServiceBuilder().GetFileUrl(user, recipes).Build();

        return new GetDashboardUseCase(loggedUser, mapper, repository, blobStorage);
    }
}
