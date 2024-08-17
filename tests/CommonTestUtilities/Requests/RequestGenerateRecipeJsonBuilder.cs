using Bogus;
using RecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestGenerateRecipeJsonBuilder
{
    public static RequestGenerateRecipeJson Build(byte count = 5)
    {
        return new Faker<RequestGenerateRecipeJson>()
            .RuleFor(r => r.Ingredients, f => f.Make(count, () => f.Commerce.ProductName()));
    }
}
