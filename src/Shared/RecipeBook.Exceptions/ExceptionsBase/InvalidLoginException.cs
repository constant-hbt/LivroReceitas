using System.Net;

namespace RecipeBook.Exceptions.ExceptionsBase;
public class InvalidLoginException : RecipeBookException
{
    public InvalidLoginException() : base(ResourceMessagesExceptions.EMAIL_OR_PASSWORD_INVALID) { }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
