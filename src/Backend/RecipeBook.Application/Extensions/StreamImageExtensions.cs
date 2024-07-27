using FileTypeChecker.Extensions;
using FileTypeChecker.Types;

namespace RecipeBook.Application.Extensions;
public static class StreamImageExtensions
{
    public static (bool isValidImage, string extension) ValidateAndGetImageExtension(this Stream stream)
    {
        var result = (false, string.Empty);

        if (stream.Is<PortableNetworkGraphic>())
            result = (true, NormalizeExtension(PortableNetworkGraphic.TypeExtension));
        else if (stream.Is<JointPhotographicExpertsGroup>())
            result = (true, NormalizeExtension(JointPhotographicExpertsGroup.TypeExtension));

        // Permite que a stream seja lida novamente, resetando a posição do ponteiro de leitura
        stream.Position = 0;

        return result;
    }

    private static string NormalizeExtension(string extension)
    {
        return extension.StartsWith('.') ? extension : $".{extension}";
    }
}
