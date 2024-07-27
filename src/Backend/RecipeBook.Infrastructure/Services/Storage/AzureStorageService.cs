using Azure.Storage.Blobs;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Services.Storage;

namespace RecipeBook.Infrastructure.Services.Storage;
public class AzureStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task Upload(User user, Stream file, string fileName)
    {
        var container = _blobServiceClient.GetBlobContainerClient(user.UserIdentifier.ToString());
        await container.CreateIfNotExistsAsync();

        var blobClient = container.GetBlobClient(fileName);

        // Caso passe o nome de um arquivo que já existe, ele irá sobrescrever
        await blobClient.UploadAsync(file, overwrite: true);
    }
}
