using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace RecipeBook.API.Converters;

public partial class StringConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString()?.Trim();

        if (value is null) 
            return null;

        return RemoveExtraWhiteSpaces().Replace(value, " ");
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }

    // O código com o atributo [GeneratedRegex(@"\s+")] significa que o compilador vai gerar um método que cria um objeto Regex
    // para a expressão regular \s+ (um ou mais espaços em branco) de maneira otimizada e que permite reutilizar a expressão regular.
    // É um recurso introduzido no .NET 7
    [GeneratedRegex(@"\s+")]
    private static partial Regex RemoveExtraWhiteSpaces();
}
