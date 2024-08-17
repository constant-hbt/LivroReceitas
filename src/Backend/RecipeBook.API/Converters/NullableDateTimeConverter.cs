using System.Text.Json;
using System.Text.Json.Serialization;

namespace RecipeBook.API.Converters;

public class NullableDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        var dateTime = reader.GetDateTime();
        return dateTime.ToUniversalTime(); // Converter para UTC
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(value.Value.ToUniversalTime());
        else
            writer.WriteNullValue();
    }
}
