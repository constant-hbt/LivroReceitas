using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using RecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;
using Xunit;

namespace WebApi.Test.Recipe.Delete;

public class DeleteRecipeTest : RecipeBookClassFixture
{
    private const string METHOD = "recipe";
    private readonly Guid _userIdentifier;
    private readonly string _recipeId;

    public DeleteRecipeTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.UserIdentifier;
        _recipeId = factory.GetRecipeId();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoDelete($"{METHOD}/{_recipeId}", token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        response = await DoGet($"{METHOD}/{_recipeId}", token);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Recipe_Not_Found(string culture)
    {
        var id = IdEncripterBuilder.Build().Encode(1000);
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoDelete($"{METHOD}/{id}", token: token, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        using var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesExceptions.ResourceManager.GetString(nameof(ResourceMessagesExceptions.RECIPE_NOT_FOUND), new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
    }
}
