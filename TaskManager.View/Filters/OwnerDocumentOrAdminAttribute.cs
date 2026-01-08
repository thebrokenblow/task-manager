using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Domain.Queries;
using TaskManager.Domain.Services;
using TaskManager.View.Controllers;
using TaskManager.View.Utilities;

namespace TaskManager.View.Filters;

/// <summary>
/// Атрибут для ограничения доступа владельцу документа или администратору.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class OwnerDocumentOrAdminAttribute : Attribute, IAsyncAuthorizationFilter
{
    /// <summary>
    /// Проверяет права доступа пользователя к документу.
    /// </summary>
    /// <param name="context">Контекст авторизации.</param>
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var authService = context.HttpContext.RequestServices.GetService<IAuthService>() ??
                                    throw new InvalidOperationException("Не зарегистрирован сервис IAuthService");

        var documentQuery = context.HttpContext.RequestServices.GetService<IDocumentQuery>() ??
                                    throw new InvalidOperationException("Не зарегистрирован сервис IDocumentQuery");

        if (!authService.IsAuthenticated)
        {
            RedirectToLogin(context);
            return;
        }

        if (authService.IsAdmin)
        {
            return;
        }

        if (!context.HttpContext.Request.Path.HasValue)
        {
            context.Result = new BadRequestResult();
            return;
        }

        var pathSegments = context.HttpContext.Request.Path.Value.Split('/');
        var idValue = pathSegments.Last();

        if (!int.TryParse(idValue, out int id))
        {
            if (!context.RouteData.Values.TryGetValue("id", out var routeId) || routeId == null)
            {
                context.Result = new BadRequestResult();
                return;
            }

            if (!int.TryParse(routeId.ToString(), out id))
            {
                context.Result = new BadRequestResult();
                return;
            }
        }

        var createdByEmployeeId = await documentQuery.GetIdEmployeeCreatedAsync(id);

        if (createdByEmployeeId == null)
        {
            RedirectToDocumentNotFoundError(context);
            return;
        }

        if (createdByEmployeeId.Value != authService.IdCurrentUser)
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
    /// Перенаправляет пользователя на страницу ошибки "Документ не найден".
    /// Используется при попытке доступа к несуществующему или удаленному документу.
    /// </summary>
    /// <param name="context">Контекст фильтра авторизации, содержащий информацию о текущем запросе.</param>
    private void RedirectToDocumentNotFoundError(AuthorizationFilterContext context)
    {
        var nameController = NameController.GetControllerName(nameof(ErrorsController));
        var actionController = nameof(ErrorsController.DocumentNotFoundError);

        context.Result = new RedirectToActionResult(actionController, nameController, null);
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