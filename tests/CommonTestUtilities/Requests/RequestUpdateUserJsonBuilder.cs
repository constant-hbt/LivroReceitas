using Bogus;
using RecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(u => u.Name, (f) => f.Person.FirstName)
            .RuleFor(u => u.Email, (f) => f.Internet.Email());
    }
}
