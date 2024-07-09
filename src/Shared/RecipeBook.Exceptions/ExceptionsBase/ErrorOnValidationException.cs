using System.Net;

namespace RecipeBook.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : RecipeBookException
{
    public ErrorOnValidationException(IList<string> errorMessages) : base(string.Empty)
    {
        _errorMessages = errorMessages ?? new List<string>();
    }

    private IList<string> _errorMessages;

    public override IList<string> GetErrorMessages() => _errorMessages;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}