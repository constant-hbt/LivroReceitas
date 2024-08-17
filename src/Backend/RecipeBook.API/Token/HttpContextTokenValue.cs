using RecipeBook.Domain.Security.Tokens;

namespace RecipeBook.API.Token;

public class HttpContextTokenValue(IHttpContextAccessor contextAccessor) : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public string Value()
    {
        var authorization = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return authorization["Bearer ".Length..].Trim();
    }
}
