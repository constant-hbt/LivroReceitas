using Bogus;
using RecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;
//public class RequestRegisterUserJsonBuilder
//{
//    public static RequestRegisterUserJson Build()
//    {
//        var faker = new Faker<RequestRegisterUserJson>()
//                .CustomInstantiator(f =>
//                {
//                    var name = f.Person.FirstName;
//                    var email = f.Internet.Email(name);
//                    var password = f.Internet.Password();

//                    return new RequestRegisterUserJson(name, email, password);
//                });

//        return faker.Generate();
//    }
//}

public class RequestRegisterUserJsonBuilder : Faker<RequestRegisterUserJson>
{
    public RequestRegisterUserJsonBuilder(int passwordLength = 10)
    {
        CustomInstantiator(f =>
        {
            var name = f.Person.FirstName;
            var email = f.Internet.Email(name);
            var password = f.Internet.Password(passwordLength);

            return new RequestRegisterUserJson(name, email, password);
        });

        this.Generate();
    }

    public RequestRegisterUserJson Build()
    {
        return this.Generate();
    }
}
