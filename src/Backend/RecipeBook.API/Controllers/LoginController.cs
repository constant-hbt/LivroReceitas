using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.UseCases.Login.DoLogin;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.API.Controllers;

public class LoginController : RecipeBookBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] IDoLoginUseCase useCase, [FromBody] RequestLoginJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
