namespace RecipeBook.Communication.Responses;
public class ResponseRecipesJson
{
    public IEnumerable<ResponseShortRecipeJson> Recipes { get; set; } = [];
}
