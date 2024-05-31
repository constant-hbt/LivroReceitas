namespace RecipeBook.Domain.Entities;

public class User : EntityBase
{
    public User() { }

    public User(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }

    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;

    public void SetPassword(string password)
    {
        Password = password;
    }
}
