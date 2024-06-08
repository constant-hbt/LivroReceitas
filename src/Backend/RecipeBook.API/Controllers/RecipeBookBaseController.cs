using Microsoft.AspNetCore.Mvc;

namespace RecipeBook.API.Controllers;

[Route("[controller]")]
[ApiController]
public abstract class RecipeBookBaseController : ControllerBase
{
}
