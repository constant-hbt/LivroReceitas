using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RecipeBook.Infrastructure.DataAccess.Converters;
public class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter()
        : base(
            v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),  // Convert to UTC when writing to the database
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc))             // Specify UTC kind when reading from the database
    { }
}
