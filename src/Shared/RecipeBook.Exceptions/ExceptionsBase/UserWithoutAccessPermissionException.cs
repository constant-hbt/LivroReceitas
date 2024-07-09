using System.Net;

namespace RecipeBook.Exceptions.ExceptionsBase;
public class UserWithoutAccessPermissionException : RecipeBookException
{
    public UserWithoutAccessPermissionException() : base(ResourceMessagesExceptions.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE) { }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
