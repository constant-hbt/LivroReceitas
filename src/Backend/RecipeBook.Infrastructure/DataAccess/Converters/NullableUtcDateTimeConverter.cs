using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RecipeBook.Infrastructure.DataAccess.Converters;
public class NullableUtcDateTimeConverter : ValueConverter<DateTime?, DateTime?>
{
    public NullableUtcDateTimeConverter()
        : base(
            v => v.HasValue ? (v.Value.Kind == DateTimeKind.Utc ? v : v.Value.ToUniversalTime()) : v,  // Convert to UTC when writing to the database
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v)  // Specify UTC kind when reading from the database
    { }
}
