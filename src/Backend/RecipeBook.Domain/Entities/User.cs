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

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid UserIdentifier { get; set; } = Guid.NewGuid();

    public void SetPassword(string password)
    {
        Password = password;
    }
}
