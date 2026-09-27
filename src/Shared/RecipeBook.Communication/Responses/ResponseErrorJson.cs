namespace RecipeBook.Communication.Responses;
public class ResponseErrorJson
{
    public ResponseErrorJson(IList<string> errors)
    {
        Errors = errors ?? [];
    }

    public ResponseErrorJson(string error)
    {
        Errors =
        [
            error ?? string.Empty
        ];
    }

    public IList<string> Errors { get; }

    public bool TokenIsExpired { get; set; }
}
