using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Domain.Services;
using TaskManager.View.Controllers;
using TaskManager.View.Utils;

namespace TaskManager.View.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class AdminOnlyAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authService = context.HttpContext.RequestServices.GetService<IAuthService>() ??
            throw new InvalidOperationException("Не зарегистрирован сервис IAuthService");

        if (!authService.IsAuthenticated)
        {
            var returnUrl = context.HttpContext.Request.Path;

            var nameController = NameController.GetControllerName(nameof(AccountsController));
            var actionController = nameof(AccountsController.Login);

            context.Result = new RedirectToActionResult(
                actionController,
                nameController,
                new { returnUrl });

            return;
        }

        if (!authService.IsAdmin)
        {
            var nameController = NameController.GetControllerName(nameof(AccountsController));
            var actionController = nameof(AccountsController.AccessDenied);

            context.Result = new RedirectToActionResult(actionController, nameController, null);
        }
    }
}