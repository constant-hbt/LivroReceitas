using AutoMapper;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Services.Storage;

namespace RecipeBook.Application.Extensions;
public static class RecipeListExtension
{
    public static async Task<IList<ResponseShortRecipeJson>> MaptoShortRecipeJson(
        this IList<Recipe> recipes,
        User user,
        IBlobStorageService blobStorageService,
        IMapper mapper)
    {
        var result = recipes.Select(async recipe =>
        {
            var response = mapper.Map<ResponseShortRecipeJson>(recipe);

            if (recipe.ImageIdentifier.NotEmpty())
            {
                response.ImageUrl = await blobStorageService.GetFileUrl(user, recipe.ImageIdentifier!);
            }

            return response;
        });

        var response = await Task.WhenAll(result);

        return response;
    }
}
