using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Controllers;
using TaskManager.Services.Interfaces;
using TaskManager.Utils;

namespace TaskManager.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthenticatedUserAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();

        var path = context.HttpContext.Request.Path;
        if (!authService.IsAuthenticated)
        {
            context.Result = RedirectToAccount(path);
            return;
        }
    }

    private static RedirectToActionResult RedirectToAccount(string returnUrl)
    {
        var nameAction = nameof(AccountsController.Login);
        var fullNameController = nameof(AccountsController);
        var nameController = NameController.GetControllerName(fullNameController);

        return new RedirectToActionResult(nameAction, nameController, new { returnUrl });
    }
}