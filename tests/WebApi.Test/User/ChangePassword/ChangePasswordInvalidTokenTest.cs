using CommonTestUtilities.Tokens;
using FluentAssertions;
using RecipeBook.Communication.Requests;
using System.Net;
using Xunit;

namespace WebApi.Test.User.ChangePassword;
public class ChangePasswordInvalidTokenTest(CustomWebApplicationFactory factory) : RecipeBookClassFixture(factory)
{
    private const string METHOD = "user/change-password";

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = new RequestChangePasswordJson();

        var response = await DoPut(METHOD, request, token: "tokenInvalid");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = new RequestChangePasswordJson();

        var response = await DoPut(METHOD, request, token: string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = new RequestChangePasswordJson();
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPut(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
