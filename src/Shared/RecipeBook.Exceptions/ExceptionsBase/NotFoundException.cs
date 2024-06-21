namespace RecipeBook.Exceptions.ExceptionsBase;
public class NotFoundException(string message) : RecipeBookException(message)
{
}
