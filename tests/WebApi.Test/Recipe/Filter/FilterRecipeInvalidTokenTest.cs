using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;
using Xunit;

namespace WebApi.Test.Recipe.Filter;
public class FilterRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : RecipeBookClassFixture(factory)
{
    private const string METHOD = "recipe/filter";

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPost(METHOD, request, token: "tokenInvalid");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPost(METHOD, request, token: string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPost(METHOD, request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
