using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.UseCases.Login.DoLogin;
using RecipeBook.Application.UseCases.Login.External;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using System.Security.Claims;

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

    [HttpGet("google")]
    public async Task<IActionResult> LoginGoogle(string returnUrl, [FromServices] IExternalLoginUseCase useCase)
    {
        var authenticate = await Request.HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (IsNotAuthenticated(authenticate))
        {
            return Challenge(GoogleDefaults.AuthenticationScheme);
        }

        var claims = authenticate.Principal!.Identities.First().Claims;

        var name = claims.First(c => c.Type == ClaimTypes.Name).Value;
        var email = claims.First(c => c.Type == ClaimTypes.Email).Value;

        var token = await useCase.Execute(name, email);

        return Redirect($"{returnUrl}/{token}");
    }
}
