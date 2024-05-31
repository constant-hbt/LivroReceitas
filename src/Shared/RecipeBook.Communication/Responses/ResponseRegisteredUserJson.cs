namespace RecipeBook.Communication.Responses;

public class ResponseRegisteredUserJson(string name)
{
    public string Name { get; } = name;
}
