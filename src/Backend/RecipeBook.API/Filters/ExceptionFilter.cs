using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RecipeBook.Communication.Responses;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;

namespace RecipeBook.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is RecipeBookException recipeBookException)
            HandleProjectException(recipeBookException, context);
        else
            ThrowUnkownException(context);
    }

    private static void HandleProjectException(RecipeBookException recipeBookException, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)recipeBookException.GetStatusCode();
        context.Result = new ObjectResult(new ResponseErrorJson(recipeBookException.GetErrorMessages()));
    }

    private static void ThrowUnkownException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesExceptions.UNKNOW_ERROR));
    }
}
