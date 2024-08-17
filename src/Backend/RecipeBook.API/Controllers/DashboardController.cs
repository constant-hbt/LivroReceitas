using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Attributes;
using RecipeBook.Application.UseCases.Dashboard;
using RecipeBook.Communication.Responses;

namespace RecipeBook.API.Controllers;

[AuthenticatedUser]
public class DashboardController : RecipeBookBaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get([FromServices] IGetDashboardUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Recipes.Any())
            return Ok(response);

        return NoContent();
    }
}
