using TaskManager.View.Controllers;
using TaskManager.View.Utils;

namespace TaskManager.View.Middlewares;


public class GlobalExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

            var nameAction = nameof(ErrorsController.UnhandledError);
            var nameController = NameController.GetControllerName(nameof(ErrorsController));

            context.Response.Redirect($"/{nameController}/{nameAction}");
        }
    }
}