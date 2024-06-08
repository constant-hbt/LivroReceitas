using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.API.Controllers;

public class UserController : RecipeBookBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromServices] IRegisterUseCase useCase, [FromBody] RequestRegisterUserJson request)
    {
        var resultado = await useCase.Execute(request);

        return Created(string.Empty, resultado);
    }
}
