using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.UseCases.Token.RefreshToken;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.API.Controllers;

public class TokenController : RecipeBookBaseController
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken(
        [FromServices] IUseRefreshTokenUseCase useCase,
        [FromBody] RequestNewTokenJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
