namespace RecipeBook.Exceptions.ExceptionsBase;

public class RecipeBookException : SystemException
{
    protected RecipeBookException() { }

    public RecipeBookException(string message) : base(message) { }
}
