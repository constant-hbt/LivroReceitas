namespace RecipeBook.Domain.Entities;
public class DishType : EntityBase
{
    public long RecipeId { get; set; }
    public Enums.DishType Type { get; set; }
}
