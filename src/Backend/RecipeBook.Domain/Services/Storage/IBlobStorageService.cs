using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Services.Storage;
public interface IBlobStorageService
{
    Task<string> GetImageUrl(User user, string fileName);
    Task Upload(User user, Stream file, string fileName);
}
