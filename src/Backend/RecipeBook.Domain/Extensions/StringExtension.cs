using System.Diagnostics.CodeAnalysis;

namespace RecipeBook.Domain.Extensions;
public static class StringExtension
{
    // O atributo [NotNullWhen(true)] informa ao compilador que está garantindo que não será nulo quando a função retornar true
    public static bool NotEmpty([NotNullWhen(true)] this string? value) => !string.IsNullOrWhiteSpace(value);

    public static bool IsEmpty([NotNullWhen(true)] this string? value) => string.IsNullOrWhiteSpace(value);
}
