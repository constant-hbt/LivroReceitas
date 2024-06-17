namespace RecipeBook.Domain.Entities;
public class Ingredient : EntityBase
{
    public long RecipeId { get; set; }
    public string Item {  get; set; } = string.Empty;
}
