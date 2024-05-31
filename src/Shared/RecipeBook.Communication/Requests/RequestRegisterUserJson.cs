namespace RecipeBook.Communication.Requests;

public class RequestRegisterUserJson(string name, string email, string password)
{
    public string Name { get; } = name;
    public string Email { get; } = email;
    public string Password { get; } = password;
}
