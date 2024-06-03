namespace RecipeBook.Domain.Entities;

public class EntityBase
{
    public long Id { get; private set;  }
    public bool Active { get; private set; } = true;
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
}
