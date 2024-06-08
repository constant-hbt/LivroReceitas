namespace RecipeBook.Exceptions.ExceptionsBase;
public class InvalidLoginException : RecipeBookException
{
    public InvalidLoginException() : base(ResourceMessagesExceptions.EMAIL_OR_PASSWORD_INVALID) { }
}
