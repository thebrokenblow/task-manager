using TaskManager.View.Controllers;
using TaskManager.View.Utilities;

namespace TaskManager.View.Middlewares;


public sealed class GlobalExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (UnauthorizedAccessException)
        {
            RedirectToUnauthorizedError(context);

            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            RedirectToUnhandledError(context);

            return;
        }
    }

    private void RedirectToUnauthorizedError(HttpContext context)
    {
        var nameAction = nameof(AccountsController.Login);
        var nameController = NameController.GetControllerName(nameof(AccountsController));

        context.Response.Redirect($"/{nameController}/{nameAction}");
    }

    private void RedirectToUnhandledError(HttpContext context)
    {
        var nameAction = nameof(ErrorsController.UnhandledError);
        var nameController = NameController.GetControllerName(nameof(ErrorsController));

        context.Response.Redirect($"/{nameController}/{nameAction}");
    }
}