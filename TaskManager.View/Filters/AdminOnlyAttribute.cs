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
            RedirectToLogin(context);
            return;
        }

        if (!authService.IsAdmin)
        {
            RedirectToAccessDeniedError(context);
        }
    }

    /// <summary>
    /// Перенаправляет пользователя на страницу входа (авторизации) в систему.
    /// Сохраняет текущий URL в параметре returnUrl для последующего возврата после успешной аутентификации.
    /// </summary>
    /// <param name="context">Контекст фильтра авторизации, содержащий информацию о текущем запросе.</param>
    /// <remarks>
    /// Используется для перенаправления неавторизованных пользователей на страницу входа
    /// с сохранением исходного запрашиваемого URL для возврата после успешной авторизации.
    /// </remarks>
    private void RedirectToLogin(AuthorizationFilterContext context)
    {
        var returnUrl = context.HttpContext.Request.Path;
        var nameController = NameController.GetControllerName(nameof(AccountsController));
        var actionController = nameof(AccountsController.Login);

        context.Result = new RedirectToActionResult(actionController, nameController, new { returnUrl });
    }

    /// <summary>
    /// Перенаправляет пользователя на страницу ошибки "Доступ запрещен".
    /// Используется при попытке доступа к ресурсу без необходимых прав доступа.
    /// </summary>
    /// <param name="context">Контекст фильтра авторизации, содержащий информацию о текущем запросе.</param>
    private void RedirectToAccessDeniedError(AuthorizationFilterContext context)
    {
        var nameController = NameController.GetControllerName(nameof(ErrorsController));
        var actionController = nameof(ErrorsController.AccessDeniedError);

        context.Result = new RedirectToActionResult(actionController, nameController, null);
    }
}