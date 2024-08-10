using System.Net;

namespace RecipeBook.Exceptions.ExceptionsBase;

public class RefreshTokenExpiredException() : RecipeBookException(ResourceMessagesExceptions.REFRESH_TOKEN_EXPIRED)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
