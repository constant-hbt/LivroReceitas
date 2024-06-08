namespace RecipeBook.Exceptions.ExceptionsBase;

public abstract class RecipeBookException : SystemException
{
    protected RecipeBookException() { }

    protected RecipeBookException(string message) : base(message) { }
}
