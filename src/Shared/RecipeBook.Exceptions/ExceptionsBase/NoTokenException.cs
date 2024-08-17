using System.Net;

namespace RecipeBook.Exceptions.ExceptionsBase;
public class NoTokenException : RecipeBookException
{
    public NoTokenException() : base(ResourceMessagesExceptions.NO_TOKEN) { }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
