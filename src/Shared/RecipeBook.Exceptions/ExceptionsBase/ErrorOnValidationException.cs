namespace RecipeBook.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : RecipeBookException
{
    public ErrorOnValidationException(IList<string> errorMessages)
    {
        ErrorMessages = errorMessages ?? new List<string>();
    }

    public IList<string> ErrorMessages { get; }
}