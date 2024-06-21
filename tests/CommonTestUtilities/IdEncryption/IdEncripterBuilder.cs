using Sqids;

namespace CommonTestUtilities.IdEncryption;
public class IdEncripterBuilder
{
    public static SqidsEncoder<long> Build()
    {
        return new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = "MgHEkfajRQ7lPydCBDOW3NmxqovnwK8IJ9FGtbZrXL0eTiVUuS4pYA1z5h26cs"
        });
    }
}
