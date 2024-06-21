using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;
using Xunit;

namespace WebApi.Test.Recipe.Update;

public class UpdateRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : RecipeBookClassFixture(factory)
{
    private const string METHOD = "recipe";

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var id = IdEncripterBuilder.Build().Encode(1);
        var request = RequestRecipeJsonBuilder.Build();

        var response = await DoPut($"{METHOD}/{id}", request, token: "tokenInvalid");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var id = IdEncripterBuilder.Build().Encode(1);
        var request = RequestRecipeJsonBuilder.Build();

        var response = await DoPut($"{METHOD}/{id}", request, token: string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var id = IdEncripterBuilder.Build().Encode(1);
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
        var request = RequestRecipeJsonBuilder.Build();

        var response = await DoPut($"{METHOD}/{id}", request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}