using CommonTestUtilities.Tokens;
using FluentAssertions;
using RecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;
using Xunit;

namespace WebApi.Test.User.Profile;
public class GetUserProfileInvalidTokenTest : RecipeBookClassFixture
{
    private const string METHOD = "user";

    public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Invalid(string culture)
    {
        var response = await DoGet(METHOD, "tokenInvalid", culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesExceptions.ResourceManager.GetString(nameof(ResourceMessagesExceptions.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE), new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Token(string culture)
    {
        var response = await DoGet(METHOD, string.Empty, culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesExceptions.ResourceManager.GetString(nameof(ResourceMessagesExceptions.NO_TOKEN), new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_With_User_NotFound(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoGet(METHOD, token, culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesExceptions.ResourceManager.GetString(nameof(ResourceMessagesExceptions.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE), new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
