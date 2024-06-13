using Microsoft.AspNetCore.Mvc;
using RecipeBook.API.Attributes;
using RecipeBook.Application.UseCases.User.Profile;
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

    [HttpGet]
    [AuthenticatedUser]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        var result = await useCase.Execute();

        return Ok(result);
    }
}
