using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;
using Xunit;

namespace WebApi.Test.Dashboard;
public class GetDashboardInvalidTokenTest(CustomWebApplicationFactory factory) : RecipeBookClassFixture(factory)
{
    private const string METHOD = "dashboard";

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var response = await DoGet(METHOD, token: "tokenInvalid");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var response = await DoGet(METHOD, token: string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoGet(METHOD, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
