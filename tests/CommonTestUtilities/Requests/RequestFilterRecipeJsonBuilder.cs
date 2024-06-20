using Bogus;
using RecipeBook.Communication.Enums;
using RecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestFilterRecipeJsonBuilder
{
    public static RequestFilterRecipeJson Build()
    {
        return new Faker<RequestFilterRecipeJson>()
            .RuleFor(r => r.RecipeTitle_Ingredient, (f) => f.Lorem.Word())
            .RuleFor(r => r.CookingTimes, (f) => f.Make(1, () => f.PickRandom<CookingTime>()))
            .RuleFor(r => r.DishTypes, (f) => f.Make(1, () => f.PickRandom<DishType>()))
            .RuleFor(r => r.Difficulties, (f) => f.Make(1, () => f.PickRandom<Difficulty>()));
    }
}
