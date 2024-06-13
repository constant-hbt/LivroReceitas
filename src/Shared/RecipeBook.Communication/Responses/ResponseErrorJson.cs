namespace RecipeBook.Communication.Responses;
public class ResponseErrorJson
{
    public ResponseErrorJson(IList<string> errors)
    {
        Errors = errors ?? new List<string>();
    }

    public ResponseErrorJson(string error)
    {
        Errors = new List<string>
        {
            error ?? string.Empty
        };
    }

    public IList<string> Errors { get; }

    public bool TokenIsExpired { get; set; }
}
