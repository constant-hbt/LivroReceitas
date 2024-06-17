namespace RecipeBook.Domain.Entities;
public class Instruction : EntityBase
{
    public long RecipeId { get; set; }
    public int Step { get; set; }
    public string Text { get; set; } = string.Empty;
}
