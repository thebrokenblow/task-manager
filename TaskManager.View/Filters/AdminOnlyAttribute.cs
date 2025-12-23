using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Domain.Services;
using TaskManager.View.Controllers;
using TaskManager.View.Utilities;

namespace TaskManager.View.Filters;

/// <summary>
/// Атрибут для ограничения доступа только для администраторов.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class AdminOnlyAttribute : Attribute, IAuthorizationFilter
{
    /// <summary>
    /// Проверяет права доступа пользователя.
    /// </summary>
    /// <param name="context">Контекст авторизации.</param>
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
            var actionController = nameof(ErrorsController.AccessDeniedError);

            context.Result = new RedirectToActionResult(actionController, nameController, null);
        }
    }
}