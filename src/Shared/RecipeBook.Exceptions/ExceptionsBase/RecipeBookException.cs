using System.Net;

namespace RecipeBook.Exceptions.ExceptionsBase;

public abstract class RecipeBookException : SystemException
{
    protected RecipeBookException() { }

    public RecipeBookException(string message) : base(message) { }

    public abstract IList<string> GetErrorMessages();

    public abstract HttpStatusCode GetStatusCode();
}
