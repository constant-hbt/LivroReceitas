using RecipeBook.Domain.Dtos;

namespace RecipeBook.Domain.Services.OpenAI;
public interface IGenerateRecipeAI
{
    Task<GeneratedRecipeDto> Generate(IList<string> ingredients);
}
