using System.Net;

namespace RecipeBook.Exceptions.ExceptionsBase;
public class RefreshTokenNotFoundException() : RecipeBookException(ResourceMessagesExceptions.REFRESH_TOKEN_NOT_FOUND)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
