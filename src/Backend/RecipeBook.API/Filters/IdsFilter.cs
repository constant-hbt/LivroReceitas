using Microsoft.OpenApi.Models;
using RecipeBook.API.Binders;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RecipeBook.API.Filters;

public class IdsFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Recupera todos os endpoints que o swagger mapeou e que possuem o ModelBinder RecipeBookIdBinder
        var encryptedIds = context.ApiDescription.ParameterDescriptions
                            .Where(x => x.ModelMetadata.BinderType == typeof(RecipeBookIdBinder))
                            .ToDictionary(d => d.Name, d => d);

        foreach (var parameter in operation.Parameters)
        {
            if (encryptedIds.TryGetValue(parameter.Name, out var apiParameter))
            {
                parameter.Schema.Format = string.Empty;
                parameter.Schema.Type = "string";
            }
        }

        foreach (var schema in context.SchemaRepository.Schemas.Values)
        {
            foreach (var property in schema.Properties)
            {
                if (encryptedIds.TryGetValue(property.Key, out var apiParameter))
                {
                    property.Value.Format = string.Empty;
                    property.Value.Type = "string";
                }
            }
        }
    }
}
