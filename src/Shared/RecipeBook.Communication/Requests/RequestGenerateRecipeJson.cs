namespace RecipeBook.Communication.Requests;
public record RequestGenerateRecipeJson
{
    public IList<string> Ingredients { get; init; } = [];
}
